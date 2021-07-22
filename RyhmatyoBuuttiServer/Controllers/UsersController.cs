using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserRepository;
        private IMapper Mapper;
        public UsersController(IUserRepository iUserRepository, IMapper iMapper)
        {
            UserRepository = iUserRepository;
            Mapper = iMapper;
        }

        [HttpGet("users")]
        public IEnumerable<User> getAllUsers()
        {
            return UserRepository.getAllUsers();
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDTO model)
        {
            List<string> duplicates = new List<string>();

            if (UserRepository.doesEmailExist(model.Email))
            {
                duplicates.Add("This email address already exists");
            }

            if (UserRepository.doesUsernameExist(model.Username))
            {
                duplicates.Add("This username already exists");
            }

            if (duplicates.Any())
            {
                return BadRequest(duplicates);
            }

            var user = Mapper.Map<User>(model);

            user.Password = BC.HashPassword(model.Password);

            UserRepository.AddUser(user);
            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO model)
        {
            var user = UserRepository.findUserToAuthenticate(model.Email);
            if (user == null || !BC.Verify(model.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid username or password." });
            }

            //var jwtToken = generateJwtToken(user);
            //var response = Mapper.Map<UserAuthenticateResponse>(user);
            //response.JwtToken = jwtToken;

            return Ok(new { message = "Successfully logged in." });
        }

        //private string generateJwtToken(User user)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes("57053634-6a5a-45fa-a607-3444801f1ea4");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
        //        Expires = DateTime.UtcNow.AddMinutes(15),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}
