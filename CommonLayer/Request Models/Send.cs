using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Request_Models
{
    public class Send
    {
        public string SendMail(string ToEmail,string Token)
        {
            string FromEmail = "mdghouse23102@gmail.com";
            MailMessage Message = new MailMessage(FromEmail,ToEmail);
            string MailBody = "the token for the reset Password : " + Token;
            Message.Subject = "Token Generated for resetting Password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            NetworkCredential credential = new
                NetworkCredential("mdghouse23102@gmail.com", "dhdx ceco hzsv dlon");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(Message);

            return ToEmail;
        }
    }
}
