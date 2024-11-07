using CommonLayer.Request_Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private IUserRepo repo;

        public UserManager(IUserRepo repo)
        {
            this.repo = repo;
        }

        public UserEntity Registration(RegistrationModel user)
        {
            return repo.Registration(user);
        }
        public bool IsRegistered(string email)
        {
            return repo.IsRegistered(email);
        }

        public string Login(LoginRequest loginRequest)
        {
            return repo.Login(loginRequest);
        }
        public ForgetPasswordModel ForgetPassword(string email)
        {
            return repo.ForgetPassword(email);
        }
        public bool ResetPassword(string email, ResetPassword resetPassword)
        {
            return repo.ResetPassword(email, resetPassword);
        }
    }
}
