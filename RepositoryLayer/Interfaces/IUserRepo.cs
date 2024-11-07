using CommonLayer.Request_Models;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        UserEntity Registration(RegistrationModel model);
        public bool IsRegistered(string email);
        public string Login(LoginRequest loginRequest);
        public ForgetPasswordModel ForgetPassword(string email);
        public bool ResetPassword(string email, ResetPassword resetPassword);
    }
}