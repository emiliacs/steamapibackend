using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        private readonly IUserRepository _userRepository;

        private readonly IGameService _gameService;
        public IConfiguration _configuration { get; }
        public GameController(IGameRepository gameRepository, IUserRepository userRepository, IConfiguration configuration, IGameService gameService)
        {
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _gameService = gameService;
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

        [HttpPatch("{appid}/extradata")]
        public async Task<ActionResult> GetExtraGameData(int appid)
        {
            var game = _gameRepository.FindGame(appid);
            if (game == null)
            {
                return NotFound(new { message = "Game not found." });
            }
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(Constants.GameExtraDataUrl(appid));
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var result = await response.Content.ReadAsStringAsync();
            var jsoNData = JToken.Parse(result).First.First;
            if (!bool.Parse(jsoNData["success"].ToString()))
            {
                return BadRequest();
            }
            var gameDetailsDto = jsoNData["data"].ToObject<GameDetailsDto>();
            _gameService.AddGenres(game, gameDetailsDto);
            _gameService.AddDevelopers(game, gameDetailsDto);
            _gameService.AddPublishers(game, gameDetailsDto);
            _gameService.AddReleaseYear(game, gameDetailsDto);
            _gameRepository.UpdateGame(game);

            return Ok(gameDetailsDto);
        }



    }

}

