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
using Microsoft.Extensions.Configuration;

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
        private IGameRepository GameRepository;

        public IConfiguration Configuration { get; }
        private readonly int verificationCodeLength = 8, resetCodeLength = 8, expiresInHours = 24;
        public UsersController(IUserRepository iUserRepository, IMapper iMapper, IJWTAuthenticationManager iJWTAuthenticationManager, IUserService iUserService, IEmailService iEmailService, IConfiguration configuration, IGameRepository gameRepository)
        {
            UserRepository = iUserRepository;
            Mapper = iMapper;
            JWTAuthenticationManager = iJWTAuthenticationManager;
            UserService = iUserService;
            EmailService = iEmailService;
            Configuration = configuration;
            GameRepository = gameRepository;

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

            if (!BC.Verify(passwordChangeDTO.CurrentPassword, user.Password))
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

        [HttpPatch("users/{steamId}/addsteamgames")]
        public IActionResult AddUsersSteamGames(string steamId)
        {
            var user = UserRepository.finduserBySteamId(steamId);
            string apiKey = Configuration.GetValue<string>("SteamApiKey");
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            GetRecentlyPlayedGame(user, apiKey);
            string url = Constants.PlayerOwnedGamesUrl(apiKey, steamId);
            var gameData = JsonDataSerializerService._download_serialized_json_data<GetOwnedGames.Rootobject>(url).response.games;
            if (gameData == null)
            {
                return NotFound(new { message = "No games found." });
            }
            List<UserGame> userGames = new List<UserGame>();
            foreach (var game in gameData)
            {
                var foundGame = GameRepository.FindGame(game.appid);
                if (foundGame == null)
                {
                    Game newGame = new Game();
                    newGame.SteamId = game.appid;
                    newGame.Name = game.name;
                    GameRepository.AddGame(newGame);
                    userGames = AddUserGames(newGame, userGames, game);
                }
                else userGames = AddUserGames(foundGame, userGames, game);


            }
            user.Games = userGames;
            UserRepository.UpdateUser(user);
            return Ok(user);
        }

        [HttpGet("users/{userId}/getusersgames")]
        public IActionResult GetAllGamesOfUser(long userId)
        {
            var user = UserRepository.ReturnGamesOfUser(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            else return Ok(user);
        }

        [HttpGet("users/{userId}/getfriends")]
        public IActionResult GetAllFriendsOfUser(long userId)
        {
            var user = UserRepository.ReturnFriendsOfuser(userId);
            var userDto = Mapper.Map<UserDTO>(user);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            else return Ok(userDto);
        }

        [HttpPost("users/{userId:long}/addfriend")]
        public IActionResult AddFriend(long userId)
        {
            if (userId != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }
            User user = UserRepository.findUser(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            string friendName = HttpContext.Request.Query["name"];
            var friend = UserRepository.FindUserByName(friendName);
            if (friend == null)
            {
                return NotFound(new { message = "User not found." });
            }
            var invalidFriend = UserRepository.GetById(friend.Id, userId);
            if (invalidFriend != null || user.Id == friend.Id)
            {
                return Conflict(new { message = "User can not be added as a friend" });
            }
            Friend newFriend = new Friend
            {
                FriendEntityId = friend.Id,
                UserEntityId = userId,
                FriendName = friendName,
                RecentlyPlayedGame = friend.RecentlyPlayedGame,
                RecentlyPlayedMinutes = friend.RecentlyPlayedMinutes
            };
            UserRepository.AddFriend(newFriend);
            return Ok(new { message = "Friend has been added" });
        }

        [HttpDelete("users/{userId:long}/deletefriend")]
        public IActionResult DeleteFriend(long userId)
        {
            if (userId != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }
            User user = UserRepository.findUser(userId);
            string friendName = HttpContext.Request.Query["name"];
            var friend = UserRepository.FindUserByName(friendName);
            if (user == null || friend == null)
            {
                return NotFound(new { message = "User not found." });
            }
            var friendShipToDelete = UserRepository.GetById(friend.Id, user.Id);
            if (friendShipToDelete == null)
            {
                return Conflict(new { message = "This friend can not be deleted" });
            }
            UserRepository.DeleteFriend(friendShipToDelete);
            return Ok("Friend has been deleted successfully");
        }

        private List<UserGame> AddUserGames(Game newGame, List<UserGame> userGames, GetOwnedGames.Game game)
        {
            var userGameFound = GameRepository.FindUserGame(newGame.SteamId);
            if (userGameFound == null)
            {
                UserGame userGame = new UserGame();
                userGame.Game = newGame;
                userGame.PlayedHours = game.playtime_forever;
                userGames.Add(userGame);
                GameRepository.AddUserGame(userGame);
            }
            else
            {
                userGameFound.PlayedHours = game.playtime_forever;
                userGames.Add(userGameFound);
                GameRepository.UpdateUserGame(userGameFound);

            }
            return userGames;
        }

        private void GetRecentlyPlayedGame(User user, string apiKey)
        {
            string url = Constants.GetRecentlyPlayedGames(apiKey, user.SteamId);
            var jsondata = JsonDataSerializerService._download_serialized_json_data<RecentlyPlayedGameDTO.Rootobject>(url).response.games;
            if (jsondata != null)
            {
                foreach (var item in jsondata)
                {
                    user.RecentlyPlayedGame = item.name;
                    user.RecentlyPlayedMinutes = item.playtime_2weeks;
                }
                UserRepository.UpdateUser(user);
            }
        }
    }
}

