using NUnit.Framework;
using UserService = RyhmatyoBuuttiServer.Services.UserService;

namespace RyhmatyoBuuttiServer.UnitTests
{
    public class UserServiceTests
    {
        [TestCase(8, 8)]
        public void GenerateAccessCode_RequestNDigitsLongCode_ShouldReturnNDigitsLongCode(int n, int expected)
        {
            UserService userService = new UserService();
            Assert.AreEqual(expected: 8, actual: userService.GenerateAccessCode(8).Length);
        }           
    }
}