using AuthorizationServer.Configurations;
using System.Net;
using System.Net.Mail;

namespace AuthorizationServer.Utilities.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(string[] to, string subject, string body)
        {
            /*var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);*/
            //send email
            MailMessage message = new MailMessage(_emailConfig.From!, string.Join(",", to), subject, body);  
            message.IsBodyHtml = true;

            var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
            {
                Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password),
                EnableSsl = true
            };

            client.SendMailAsync(message);
            
        }
    }
}
