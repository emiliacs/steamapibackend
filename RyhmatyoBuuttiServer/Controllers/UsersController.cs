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
        private IUserService UserService;
        private IEmailService EmailService;
        private readonly int verificationCodeLength = 8, resetCodeLength = 8, expiresInHours = 24;
        public UsersController(IUserRepository iUserRepository, IMapper iMapper, IJWTAuthenticationManager iJWTAuthenticationManager, IUserService iUserService, IEmailService iEmailService)
        {
            UserRepository = iUserRepository;
            Mapper = iMapper;
            JWTAuthenticationManager = iJWTAuthenticationManager;
            UserService = iUserService;
            EmailService = iEmailService;
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            return Ok(UserRepository.getAllUsers());
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

            string verificationCode = UserService.GenerateAccessCode(verificationCodeLength);
            user.VerificationCode = BC.HashPassword(verificationCode);
            user.VerificationCodeExpires = DateTime.Now.AddHours(expiresInHours);
            
            UserRepository.AddUser(user);

            string message = EmailService.welcomeMessage(verificationCode);
            EmailService.Send(
                to: user.Email,
                subject: "Ryhmatyo Buutti - Registration - Verify your user account",
                text: message
                );

            return Ok(new { message = "User registered successfully. Please check your email address to verify the user account." });
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO model)
        {
            User loginUser = UserRepository.findUserByEmail(model.Email);
            
            if (loginUser == null || !BC.Verify(model.Password, loginUser.Password))
            {
                return BadRequest(new { message = "Invalid username or password." });
            }

            if (!loginUser.Verified)
            {
                return Unauthorized(new { message = "User not verified. Please verify your user account and log in again." });
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

            string resetCode = UserService.GenerateAccessCode(resetCodeLength);
            user.ResetCode = BC.HashPassword(resetCode);
            user.ResetCodeExpires = DateTime.Now.AddHours(expiresInHours);
            UserRepository.UpdateUser(user);

            string message = EmailService.passwordResetCodeMessage(resetCode);
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

        [HttpPost("users/newverificationcode")]
        public IActionResult RequestNewVerificationCode(UserForgottenPasswordDTO model)
        {
            User user = UserRepository.findUserByEmail(model.Email);

            if (user == null || user.Verified)
            {
                return Ok(new { message = "New verification code sent to email: " + model.Email });
            }

            string newCode = UserService.GenerateAccessCode(verificationCodeLength);
            user.VerificationCode = BC.HashPassword(newCode);
            user.VerificationCodeExpires = DateTime.Now.AddHours(expiresInHours);
            UserRepository.UpdateUser(user);

            string message = EmailService.newVerificationCodeMessage(newCode);
            EmailService.Send(
                to: user.Email,
                subject: "Ryhmatyo Buutti - Verify your user account",
                text: message
                );

            return Ok(new { message = "New verification code sent to email: " + model.Email });
        }

        [HttpPatch("users/verify")]
        public IActionResult VerifyUser(JsonPatchDocument<UserVerificationDTO> verificationUpdates) 
        {
            UserVerificationDTO verificationDTO = new UserVerificationDTO();
            verificationUpdates.ApplyTo(verificationDTO, ModelState);
            User user = UserRepository.findUserByEmail(verificationDTO.Email);

            if (user == null || user.Verified || !BC.Verify(verificationDTO.VerificationCode, user.VerificationCode) || DateTime.Now > user.VerificationCodeExpires)
            {
                return BadRequest(new { message = "Invalid user email address or verification code or this user is already verified." });
            }

            user.Verified = true;
            user.VerificationCode = null;
            user.VerificationCodeExpires = null;
            UserRepository.UpdateUser(user);

            string message = EmailService.userVerifiedMessage();
            EmailService.Send(
                to: user.Email,
                subject: "Ryhmatyo Buutti - User account verified",
                text: message
                );

            return Ok(new { message = "User account successfully verified. You can now log in." });
        }
    }
}
