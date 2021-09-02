using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System.Collections.Generic;
using System.Security.Claims;
using UserController = RyhmatyoBuuttiServer.Controllers.UsersController;

namespace RyhmatyoBuuttiServer.UnitTests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void GetAllUsers_FromUserRepository_ReturnsOk()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.getAllUsers()).Returns(GetTestUsers());
            var controller = new UserController(mockRepo.Object, null, null, null, null);

            var result = controller.GetAllUsers();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void DeleteUser_AuthorizedUserDeletesOwnAccount_ReturnsOk()
        {
            var ctx = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.getAllUsers()).Returns(GetTestUsers());

            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "1", ClaimValueTypes.Integer64)
            }, "Custom");

            ctx.HttpContext.User = new ClaimsPrincipal(identity);

            var controller = new UserController(mockRepo.Object, null, null, null, null);
            controller.ControllerContext = ctx;

            var result = controller.DeleteUser(1);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        private IEnumerable<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User()
            {
                Id = 1,
                Email = "testuser1@example.com",
                Username = "Test User1",
                Password = "$2a$11$XdsLZ.oCvMLg2vhJCkIFaeIy4lwipEZs6QeA3efzuoToOAARqZyFy",
                ResetCode = "ABcd1234",
                ResetCodeExpires = System.DateTime.Now.AddHours(24),
                Verified = true,
                VerificationCode = null,
                VerificationCodeExpires = null
            });
            
            users.Add(new User()
            {
                Id = 2,
                Email = "testtester@example.com",
                Username = "Test Tester",
                Password = "$2a$11$kPe4ZqGHOOMmBYYC0SKF2.OV1wG0DexoZ/.odCV4suXKFM0b3wzSG",
                ResetCode = null,
                ResetCodeExpires = null,
                Verified = false,
                VerificationCode = "abCD1234",
                VerificationCodeExpires = System.DateTime.Now.AddHours(-24)
            });

            return users;
        }
    }
}