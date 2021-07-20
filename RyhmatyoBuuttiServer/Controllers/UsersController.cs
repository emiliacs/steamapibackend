using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult Register(UserRegistrationDTO model, string origin)
        {
            // List for collecting duplication errors
            List<string> duplicates = new List<string>();

            // Validation if there are duplicates
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

            // Map model to new user object using Automapper
            var user = Mapper.Map<User>(model);

            // Hash password
            user.Password = BC.HashPassword(model.Password);

            // Save account to database
            UserRepository.AddUser(user);
            return Ok(new { message = "Registration successful." });

        }
    }
}
