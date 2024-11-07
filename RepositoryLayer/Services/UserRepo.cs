using CommonLayer.Request_Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Helpers;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext _context;
        private readonly TokenHelper _tokenHelper;

        public UserRepo(FundooDBContext _context,TokenHelper _tokenHelper)
        {
            this._context = _context;
            this._tokenHelper = _tokenHelper;
        }

        public UserEntity Registration(RegistrationModel model)
        {
            UserEntity user = new UserEntity();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Password = EncodePasswordToBase64(model.Password);

            _context.Add(user);
            _context.SaveChanges();
            return user;
        }
        private static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public bool IsRegistered(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public string Login(LoginRequest loginRequest)
        {
            var users = _context.Users.ToList();
            var user = users.FirstOrDefault(u => u.Email == loginRequest.Username && u.Password==EncodePasswordToBase64(loginRequest.Password));

            if (users.Contains(user))
            {
                var token = _tokenHelper.GenerateJwtToken(user.UserId,user.Email);
                return token;
            }
            return null;
        }

        public ForgetPasswordModel ForgetPassword(string email)
        {
            UserEntity User = _context.Users.FirstOrDefault(x => x.Email == email);
            ForgetPasswordModel forgetPassword = new ForgetPasswordModel();
            forgetPassword.Email = User.Email;
            forgetPassword.UserId=User.UserId;
            forgetPassword.Token = _tokenHelper.GenerateJwtToken(User.UserId,User.Email);
            return forgetPassword;
        }

        public bool ResetPassword(string email,ResetPassword resetPassword)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                user.Password = EncodePasswordToBase64(resetPassword.Password);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
