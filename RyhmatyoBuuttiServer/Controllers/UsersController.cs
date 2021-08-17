using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using System;
using RyhmatyoBuuttiServer.Services;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserRepository;
        private IMapper Mapper;
        private IJWTAuthenticationManager JWTAuthenticationManager;
        private IEmailService EmailService;
        public UsersController(IUserRepository iUserRepository, IMapper iMapper, IJWTAuthenticationManager iJWTAuthenticationManager, IEmailService iEmailService)
        {
            UserRepository = iUserRepository;
            Mapper = iMapper;
            JWTAuthenticationManager = iJWTAuthenticationManager;
            EmailService = iEmailService;
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
            var loginUser = UserRepository.findUserToAuthenticate(model.Email);
            
            if (loginUser == null || !BC.Verify(model.Password, loginUser.Password))
            {
                return BadRequest(new { message = "Invalid username or password." });
            }

            var jwtToken = JWTAuthenticationManager.generateJWT(loginUser);
            var user = Mapper.Map<UserAuthenticateResponse>(loginUser);
            user.JwtToken = jwtToken;

            return Ok(new { message = "Successfully logged in.", user });
        }

        [HttpPatch("users/{id:long}")]
        public IActionResult UpdateUser(long id, JsonPatchDocument<UserUpdateDTO> userUpdates)
        {
            if (id != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }

            User user = UserRepository.findUser(id);
            UserUpdateDTO updateDTO = new UserUpdateDTO
            { Email = user.Email, Username = user.Username };
            userUpdates.ApplyTo(updateDTO, ModelState);
            TryValidateModel(updateDTO);

            if (!updateDTO.Email.Equals(user.Email) && UserRepository.doesEmailExist(updateDTO.Email))
            {
                ModelState.AddModelError("Email exists", "This email already exists.");
            }

            if (!updateDTO.Username.Equals(user.Username) && UserRepository.doesUsernameExist(updateDTO.Username))
            {
                ModelState.AddModelError("Username exists", "This username already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            user.Email = updateDTO.Email;
            user.Username = updateDTO.Username;
            UserRepository.UpdateUser(user);

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("users/{id:long}")]
        public IActionResult DeleteUser(long id)
        {
            if (id != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }

            User user = UserRepository.findUser(id);
            UserRepository.DeleteUser(user);

            return Ok(new { message = "User deleted." });
        }

        [HttpPatch("users/{id:long}/changepassword")]
        public IActionResult ChangePassword(long id, JsonPatchDocument<UserPasswordChangeDTO> passwordUpdates)
        {
            if (id != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }

            User user = UserRepository.findUser(id);
            UserPasswordChangeDTO passwordChangeDTO = new UserPasswordChangeDTO();
            passwordUpdates.ApplyTo(passwordChangeDTO, ModelState);

            if(!BC.Verify(passwordChangeDTO.CurrentPassword, user.Password))
            {
                ModelState.AddModelError("Invalid current password", "Invalid current password.");
            }

            TryValidateModel(passwordChangeDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            user.Password = BC.HashPassword(passwordChangeDTO.NewPassword);
            UserRepository.UpdateUser(user);

            return Ok(new { message = "Password changed successfully." });
        }

        [HttpPost("users/forgottenpassword")]
        public IActionResult RequestPasswordResetCode(UserForgottenPasswordDTO model)
        {
            User user = UserRepository.findUserByEmail(model.Email);

            if (user == null)
            {
                return Ok(new { message = "Password reset code sent to email: " + model.Email });
            }

            string resetCode = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int resetCodeLength = 8;
            int expiresInHours = 24;

            for (int i = 0; i < resetCodeLength; i++)
            {
                resetCode += chars[new Random().Next(chars.Length)];
            }

            user.ResetCode = BC.HashPassword(resetCode);
            user.ResetCodeExpires = DateTime.Now.AddHours(expiresInHours);
            UserRepository.UpdateUser(user);

            string message = "Hello!\n\n" +
                "Here is the code for resetting password of your user account in Ryhmatyo Buutti application.\n\n" +
                "The code is: " + resetCode + "\n\n" +
                "The code is valid for the next 24 hours.\n\n" +
                "Best,\n" +
                "Ryhmatyo Buutti team";
            EmailService.Send(
                to: user.Email,
                subject: "Ryhmatyo Buutti - Reset Password",
                text: message
                );

            return Ok(new { message = "Password reset code sent to email: " + user.Email });
        }

        [HttpPatch("users/resetpassword")]
        public IActionResult ResetPassword(JsonPatchDocument<UserPasswordResetDTO> passwordUpdates)
        {
            UserPasswordResetDTO passwordResetDTO = new UserPasswordResetDTO();
            passwordUpdates.ApplyTo(passwordResetDTO, ModelState);
            User user = UserRepository.findUserByEmail(passwordResetDTO.Email);

            if (user == null || user.ResetCode == null || user.ResetCodeExpires == null 
                || passwordResetDTO.ResetCode == null || !BC.Verify(passwordResetDTO.ResetCode, user.ResetCode) 
                || DateTime.Now > user.ResetCodeExpires)
            {
                ModelState.AddModelError("Invalid user input", "Invalid user email address or reset code.");
            }

            TryValidateModel(passwordResetDTO);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Password = BC.HashPassword(passwordResetDTO.NewPassword);
            user.ResetCode = null;
            user.ResetCodeExpires = null;
            UserRepository.UpdateUser(user);

            return Ok(new { message = "Password reset successfully. You can now log in with that password." });
        }
    }
}
