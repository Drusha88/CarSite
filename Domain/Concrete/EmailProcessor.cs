using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using Domain.Entities;


namespace Domain.Concrete
{
    public class EmailSettings
    {
        public string ServerName;
        public int ServerPort;
        public string Login;
        public string Password;

        public EmailSettings()
        {
            ServerName = ConfigurationManager.AppSettings["ServerName"];
            ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);
            Login = ConfigurationManager.AppSettings["Login"];
            Password = ConfigurationManager.AppSettings["Password"];
        }

    }
    public class EmailProcessor : IEmailProcessor
    {
        private EmailSettings emailSettings;
        public EmailProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void SendEmail(User user, string url)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = true;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Login, emailSettings.Password);

                StringBuilder body = new StringBuilder()
                .AppendLine(string.Format("Здравствуйте {0}!", user.Login))
                .AppendLine("Регистрация нового пользователя почти зевершена, для подтверждения регистрации пройдите по ссылке ниже.")
                .AppendLine("---")
                .AppendLine(string.Format("http://{0}/User/Confirm?ticket={1}", url, user.Ticket));


                MailMessage mailMessage = new MailMessage(
                    emailSettings.Login,
                    user.Email,
                    string.Format("Регистрация на сайте {0}", url),
                    body.ToString()
                    );
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch(Exception)
                {
                }
                
            }
        }
    }
}
