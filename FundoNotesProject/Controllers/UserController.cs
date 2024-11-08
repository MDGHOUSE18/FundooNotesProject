using CommonLayer;
using CommonLayer.Request_Models;
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
        //private readonly ILogger<UserController> logger;

        public UserController(IUserManager manager, IBus bus,TokenHelper tokenHelper) {
            this.manager = manager;
            this.bus = bus;
            this.tokenHelper = tokenHelper;
            
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegistrationModel user)
        {
            var isExists = manager.IsRegistered(user.Email);

            if (!isExists)
            {

                var result = manager.Registration(user);

                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Success = true, Message = "registration successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "registration not successfull", Data = null });
                }
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "An account already exists with this email address.", Data = null });
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var token = manager.Login(login);

            if (token == null)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid login credentials", Data = null });
            }
            return Ok(new ResponseModel<string> { Success = true, Message = "Login Succeesfull",Data=token});
        }




        [HttpPost]
        [Route("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
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

                    return Ok(new ResponseModel<string> { Success = true, Message = "Mail send Sucessfully", Data = null });

                }
                else
                {
                    return BadRequest(new ResponseModel<string> 
                    { 
                        Success = false,
                        Message="Acount doen't exist with this email",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Authorize(AuthenticationSchemes = "ResetPasswordScheme")]
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword( [FromBody] ResetPassword resetPassword)
        {
            // Extract the token from the Authorization header
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validate the token
            if (string.IsNullOrWhiteSpace(token) || !tokenHelper.ValidateResetPasswordToken(token))
            {
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
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid credentials. Please provide a valid credentials",
                    Data = null
                });
            }
            if (resetPassword.Password != resetPassword.ConfirmPassword)
            {
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
                return Ok(new ResponseModel<string> 
                { 
                    Success = true, 
                    Message = "Password change successful. Please remember to use your new password next time.", 
                    Data = null 
                });
            }
            return NotFound(new ResponseModel<string> 
            { 
                Success=false,
                Message= "No account found with that email address.", 
                Data= null 
            });
        }
    }
}
