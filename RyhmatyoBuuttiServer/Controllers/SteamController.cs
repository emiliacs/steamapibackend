using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using RyhmatyoBuuttiServer.Services;
using System;
using System.Net.Http;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class SteamController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private IUserRepository UserRepository;
        private ISteamService SteamService;

        public SteamController(IConfiguration configuration, IUserRepository iUserRepository, ISteamService iSteamService)
        {
            Configuration = configuration;
            UserRepository = iUserRepository;
            SteamService = iSteamService;
        }

        [HttpPatch("users/{id:long}/addsteamid")]
        public IActionResult AddSteamIdToUser(long id, JsonPatchDocument<UserAddSteamIdDTO> userUpdates)
        {
            User user = UserRepository.findUser(id);

            if (id != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }

            UserAddSteamIdDTO addSteamIdDTO = new UserAddSteamIdDTO
            { SteamId = user.SteamId };
            userUpdates.ApplyTo(addSteamIdDTO);

            TryValidateModel(addSteamIdDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.SteamId = addSteamIdDTO.SteamId;
            UserRepository.UpdateUser(user);

            return Ok(new { message = "Steam ID added to user." });
        }

        [HttpGet("users/{id}/datafromsteamapi")]
        public IActionResult GetDataFromSteamApi(long id)
        {
            User user = UserRepository.findUser(id);

            if (id != Convert.ToInt64(HttpContext.User.Identity.Name))
            {
                return Unauthorized(new { message = "Access denied." });
            }

            string apiKey = Configuration.GetValue<string>("SteamApiKey");
            string steamId = user.SteamId;

            if (steamId == null)
            {
                return BadRequest(new { message = "Steam ID not found in this account." });
            }

            HttpClient http = new HttpClient();
            JObject playerSummaries = JObject.Parse(http.GetAsync(SteamService.PlayerSummariesUrl(apiKey, steamId)).Result.Content.ReadAsStringAsync().Result);
            JObject playerOwnedGames = JObject.Parse(http.GetAsync(SteamService.PlayerOwnedGamesUrl(apiKey, steamId)).Result.Content.ReadAsStringAsync().Result);

            playerSummaries.Merge(playerOwnedGames, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });

            SteamDataObject data = JsonConvert.DeserializeObject<SteamDataObject>(playerSummaries.ToString());

            return Ok(data);
        }
    }
}
