using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        private readonly IUserRepository _userRepository;
        public IConfiguration _configuration { get; }
        public GameController(IGameRepository gameRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPatch("{steamId}")]
        public IActionResult AddUsersSteamGames(string steamId)
        {
            var user = _userRepository.finduserBySteamId(steamId);
            string apiKey = _configuration.GetValue<string>("SteamApiKey");
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            string url = Constants.PlayerOwnedGamesUrl(apiKey, steamId);
            var gameData = JsonDataSerializerService._download_serialized_json_data<GetOwnedGames.Rootobject>(url).response.games;
            if (gameData == null)
            {
                return NotFound(new { message = "No games found." });
            }
            List<Game> userGames = new List<Game>();
            foreach (var game in gameData)
            {
                var foundGame = _gameRepository.FindGame(game.appid);
                if (foundGame == null)
                {
                    Game newGame = new Game();
                    newGame.SteamId = game.appid;
                    newGame.Name = game.name;
                    _gameRepository.AddGame(newGame);
                }
                else
                {
                    userGames.Add(foundGame);
                }
                user.Games = userGames;
                _userRepository.UpdateUser(user);
            }
            return Ok(user);
        }



    }

}

