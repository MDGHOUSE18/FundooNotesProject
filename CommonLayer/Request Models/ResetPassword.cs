using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Request_Models
{
    public class ResetPassword
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
