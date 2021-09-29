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

        private readonly IGameService _gameService;
        public GameController(IGameRepository gameRepository , IGameService gameService)
        {
            _gameRepository = gameRepository;
            _gameService = gameService;
        }
        [HttpGet]
        public IActionResult GetAllGames()
        {
            return Ok(_gameRepository.GetAllGames());
        }

        [HttpGet("{appId}")]
        public IActionResult GetGameByAppId(int appId)
        {
            var game = _gameRepository.ReturnGameById(appId);
            if (game == null)
            {
                return NotFound(new { message = "Game not found." });
            }
            else return Ok(game);
        }

        [HttpPatch("{appid}")]
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
            _gameService.AddImageUrl(game, gameDetailsDto);
            _gameRepository.UpdateGame(game);
            return Ok(game);
        }



    }

}

