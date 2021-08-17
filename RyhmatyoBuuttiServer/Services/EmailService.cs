using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace RyhmatyoBuuttiServer.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string text, string from = null);
    }
    
    public class EmailService : IEmailService
    {
        public IConfiguration Configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Send(string to, string subject, string text, string from = null)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? Configuration.GetValue<string>("EmailFrom")));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = text };

            var smtp = new SmtpClient();
            smtp.Connect(Configuration.GetValue<string>("SmtpHost"), Configuration.GetValue<int>("SmtpPort"), SecureSocketOptions.StartTls);
            smtp.Authenticate(Configuration.GetValue<string>("SmtpUser"), Configuration.GetValue<string>("SmtpPass"));
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
