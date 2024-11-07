using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Request_Models
{
    public class ForgetPasswordModel
    {
        public string Email { get; set; }
        public int UserId {  get; set; }
        public string Token {  get; set; }
    }
}
