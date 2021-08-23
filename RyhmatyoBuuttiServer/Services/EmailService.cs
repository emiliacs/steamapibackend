using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;

namespace RyhmatyoBuuttiServer.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string text, string from = null);
        string welcomeMessage(string verificationCode);
        string newVerificationCodeMessage(string verificationCode);
        string userVerifiedMessage();
        string passwordResetCodeMessage(string resetCode);
    }
    
    public class EmailService : IEmailService
    {
        private string message;
        public IConfiguration Configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Send(string to, string subject, string text, string from = null)
        {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? Environment.GetEnvironmentVariable("EMAIL_FROM")));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = text };

            var smtp = new SmtpClient();
            smtp.Connect(Environment.GetEnvironmentVariable("SMTP_HOST"), Int32.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")), SecureSocketOptions.StartTls);
            smtp.Authenticate(Environment.GetEnvironmentVariable("SMTP_USER"), Environment.GetEnvironmentVariable("SMTP_PASS"));
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public string welcomeMessage(string verificationCode)
        {
            message =
                "Hello!\n\n" +
                "Welcome to user of Ryhmatyo Buutti application!\n\n" +
                "We kindly ask you to verify your account with the code below.\n\n" +
                "The code is: " + verificationCode + "\n\n" +
                "The code is valid for the next 24 hours.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";

            return message;
        }

        public string newVerificationCodeMessage(string verificationCode)
        {
            message =
                "Hello!\n\n" +
                "A new verification code was requested from this email address for the equivalent user account of Ryhmatyo Buutti application.\n\n" +
                "The code is: " + verificationCode + "\n\n" +
                "The code is valid for the next 24 hours.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";

            return message;
        }

        public string userVerifiedMessage()
        {
            message =
                "Hello!\n\n" +
                "Your username of Ryhmatyo Buutti application has now been successfully verified.\n\n" +
                "You can now use your account in the application.\n\n" +
                "We wish you nice moments with the application.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";

            return message;
        }

        public string passwordResetCodeMessage(string resetCode)
        {
            message = 
                "Hello!\n\n" +
                "Here is the code for resetting password of your user account in Ryhmatyo Buutti application.\n\n" +
                "The code is: " + resetCode + "\n\n" +
                "The code is valid for the next 24 hours.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";
           
            return message;
        }
    }
}
