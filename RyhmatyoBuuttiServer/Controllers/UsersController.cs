using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserRepository;
        private IMapper Mapper;
        private IJWTAuthenticationManager JWTAuthenticationManager;
        public UsersController(IUserRepository iUserRepository, IMapper iMapper, IJWTAuthenticationManager iJWTAuthenticationManager)
        {
            UserRepository = iUserRepository;
            Mapper = iMapper;
            JWTAuthenticationManager = iJWTAuthenticationManager;
        }

        [Authorize]
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

            var jwtToken = JWTAuthenticationManager.generateJWT(user);
            var response = Mapper.Map<UserAuthenticateResponse>(user);
            response.JwtToken = jwtToken;

            return Ok(new { message = "Successfully logged in.", jwtToken });
        }

        [HttpPatch("users/{id:long}")]
        public IActionResult UpdateUser(long id, JsonPatchDocument<UserUpdateDTO> userUpdates)
        {
            var user = UserRepository.findUser(id);
            
            if (user == null)
            {
                return NotFound();
            }
            Mapper.Map<JsonPatchDocument<User>>(userUpdates).ApplyTo(user);
            UserRepository.UpdateUser(user);
            
            return Ok(new { message = "User updated successfully."});
        }
    }
}
