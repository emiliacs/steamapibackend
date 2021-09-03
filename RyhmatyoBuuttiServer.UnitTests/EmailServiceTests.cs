using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailService = RyhmatyoBuuttiServer.Services.EmailService;

namespace RyhmatyoBuuttiServer.UnitTests
{
    [TestFixture]
   public class EmailServicetests
    {
        [TestCase("verificationcode")]
        public void WelcomeMessage_WithCorrectVerificationCode_ReturnsCorrectMessage(string verificationCode)
        {
            string message = "Hello!\n\n" +
             "Welcome to user of Ryhmatyo Buutti application!\n\n" +
             "We kindly ask you to verify your account with the code below.\n\n" +
             "The code is: " + verificationCode + "\n\n" +
             "The code is valid for the next 24 hours.\n\n" +
             "Best,\n" +
             "Ryhmatyo Buutti team";

            var mockConfig = new Mock<IConfiguration>();
            EmailService emailService = new EmailService(mockConfig.Object);
            string result = emailService.welcomeMessage("verificationcode");
            Assert.IsNotNull(result);
            Assert.AreEqual(result, message);
        }

        [TestCase("verificationcode")]
        public void newVerificationCodeMessage_WithVerificationcode_ReturnsCorrectMessage(string verificationCode)
        {
            string message =
                "Hello!\n\n" +
                "A new verification code was requested from this email address for the equivalent user account of Ryhmatyo Buutti application.\n\n" +
                "The code is: " + verificationCode + "\n\n" +
                "The code is valid for the next 24 hours.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";

            var mockConfig = new Mock<IConfiguration>();
            EmailService emailService = new EmailService(mockConfig.Object);
            string result = emailService.newVerificationCodeMessage("verificationcode");
            Assert.IsNotNull(result);
            Assert.AreEqual(result, message);
        }
    }
}
