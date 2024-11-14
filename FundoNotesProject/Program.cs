using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System.Text;
using MassTransit;
using RepositoryLayer.Helpers;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Application is starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);


// Ensure the "logs" folder exists within the project directory
var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Add services to the container.
builder.Services.AddControllers();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add endpoint API explorer
builder.Services.AddEndpointsApiExplorer();

// Database configuration for FundooDBContext
builder.Services.AddDbContext<FundooDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FundooDB"))
);

// Add Redis distributed cache
builder.Services.AddStackExchangeRedisCache(options => { 
    options.Configuration = builder.Configuration["RedisCacheUrl"]; 
});

// Dependency Injection for Managers and Repositories
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<TokenHelper>();
builder.Services.AddTransient<INotesManager, NotesManager>();
builder.Services.AddTransient<INotesRepo, NotesRepo>();
builder.Services.AddTransient<ICollaboratorsManager, CollaboratorsManager>();
builder.Services.AddTransient<ICollaboratorsRepo, CollaboratorsRepo>();
builder.Services.AddTransient<ILabelsManager, LabelsManager>();
builder.Services.AddTransient<ILabelsRepo, LabelsRepo>();

// JWT Authentication configuration
// Configure default authentication scheme for login tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Configure authentication scheme specifically for reset password tokens
builder.Services.AddAuthentication()
.AddJwtBearer("ResetPasswordScheme", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ResetPasswordKey"])),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



// Adding Swagger with JWT Authorization support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });

    // Define the security scheme for Bearer tokens in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter only the JWT token; 'Bearer' is added automatically.",
    });

    // Add security requirement to enforce Bearer token use
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// MassTransit configuration for RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.UseHealthCheck(provider);
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    }));
});
builder.Services.AddMassTransitHostedService();

// Enable Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Map Controllers
app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    // Catch setup errors
    logger.Error(ex, "Application stopped because of an exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application exit
    LogManager.Shutdown();
}
