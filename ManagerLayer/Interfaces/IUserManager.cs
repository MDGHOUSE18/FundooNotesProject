using CommonLayer.Request_Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserEntity Registration(RegistrationModel user);
        public bool IsRegistered(string email);
        public string Login(LoginRequest loginRequest);
        public ForgetPasswordModel ForgetPassword(string email);
        public bool ResetPassword(string email, ResetPassword resetPassword);
    }
}
