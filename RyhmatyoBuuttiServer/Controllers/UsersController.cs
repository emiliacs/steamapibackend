using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserRepository;
        public UsersController(IUserRepository iUserRepository)
        {
            UserRepository = iUserRepository;
        }

        [HttpGet("users")]
        public IEnumerable<User> getAllUsers()
        {
            return UserRepository.getAllUsers();
        }

        [HttpPost("createUser")]
        public void createUser(UserRegistrationDto registrationDto)
        {
            if (UserRepository.doesEmailExist(registrationDto.toUser()))
                {
                    ModelState.AddModelError("EmailExists", "Email address already exists");
                }

                if (UserRepository.doesUsernameExist(registrationDto.toUser()))
                {
                    ModelState.AddModelError("UsernameExists", "Username already exists");
                }

                if (ModelState.IsValid)
            {
                registrationDto.Password = PasswordHasher.Hash(registrationDto.Password);
                UserRepository.AddUser(registrationDto.toUser());
            }
                else
            {
                return;
            }
       
        }
    }
}
