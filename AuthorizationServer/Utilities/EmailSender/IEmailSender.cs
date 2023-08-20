using System.Net.Mail;

namespace AuthorizationServer.Utilities.EmailSender
{
    public interface IEmailSender
    {
        void SendEmail(string[] to, string subject, string body);
    }
}
