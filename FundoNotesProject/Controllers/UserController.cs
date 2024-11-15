using CommonLayer;
using CommonLayer.Request_Models;
using CommonLayer.Responses;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entities;
using RepositoryLayer.Helpers;
using RepositoryLayer.Migrations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotesProject.Controllers
{
    

    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly IBus bus;
        private readonly TokenHelper tokenHelper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager manager, IBus bus,TokenHelper tokenHelper, ILogger<UserController> logger)
        {
            this.manager = manager;
            this.bus = bus;
            this.tokenHelper = tokenHelper;
            this._logger = logger;
            _logger.LogInformation($"UserController initialized at {DateTime.Now}");
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegistrationModel user)
        {
            _logger.LogInformation($"POST request received at 'api/UserController/register' for email: {user.Email}");
            var isExists = manager.IsRegistered(user.Email);

            if (!isExists)
            {
                _logger.LogInformation($"Registering new user with email: {user.Email}");
                var result = manager.Registration(user);

                if (result != null)
                {
                    _logger.LogInformation($"Registration successful for email: {user.Email}");
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "registration successful", Data = result });
                }
                else
                {
                    _logger.LogError($"Registration failed for email: {user.Email}");
                    return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "registration not successful", Data = null });
                }
            }
            else
            {
                _logger.LogWarning($"An account already exists with email: {user.Email}");
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "An account already exists with this email address.", Data = null });
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            _logger.LogInformation($"Login attempt for user: {login.Username}");
            var token = manager.Login(login);

            if (token == null)
            {
                _logger.LogWarning($"Invalid login credentials for user: {login.Username}");
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid login credentials", Data = null });
            }

            _logger.LogInformation($"Login successful for user: {login.Username}");
            return Ok(new ResponseModel<string> { Success = true, Message = "Login successful", Data = token });
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            _logger.LogInformation($"Forget password request received for email: {Email}");
            try
            {
                if (manager.IsRegistered(Email))
                {
                    Send send = new Send();
                    ForgetPasswordModel forgetPasswordModel = manager.ForgetPassword(Email);
                    send.SendMail(forgetPasswordModel.Email, forgetPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");

                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgetPasswordModel);

                    _logger.LogInformation($"Mail sent successfully for email: {Email}");
                    return Ok(new ResponseModel<string> { Success = true, Message = "Mail sent successfully", Data = null });
                }
                else
                {
                    _logger.LogWarning($"Account doesn't exist for email: {Email}");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Account doesn't exist with this email",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred during forget password process for email: {Email}. Error: {ex.Message}");
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "ResetPasswordScheme")]
        [HttpPost]
        [Route("resetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPassword resetPassword)
        {
            _logger.LogInformation($"POST request received for password reset");

            // Extract the token from the Authorization header
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validate the token
            if (string.IsNullOrWhiteSpace(token) || !tokenHelper.ValidateResetPasswordToken(token))
            {
                _logger.LogWarning($"Invalid or expired token received.");
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid or expired token.",
                    Data = null
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email) || resetPassword == null || string.IsNullOrWhiteSpace(resetPassword.Password))
            {
                _logger.LogWarning($"Invalid credentials or empty password fields received.");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid credentials. Please provide valid credentials",
                    Data = null
                });
            }
            if (resetPassword.Password != resetPassword.ConfirmPassword)
            {
                _logger.LogWarning($"Password and confirmation password do not match.");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Passwords do not match. Please ensure both password fields are identical.",
                    Data = null
                });
            }

            var result = manager.ResetPassword(email, resetPassword);

            if (result)
            {
                _logger.LogInformation($"Password reset successful for email: {email}");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Password change successful. Please remember to use your new password next time.",
                    Data = null
                });
            }

            _logger.LogWarning($"No account found with email: {email} for password reset.");
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "No account found with that email address.",
                Data = null
            });
        }
    }
}
