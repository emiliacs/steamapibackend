using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RyhmatyoBuuttiServer.Models;
using RyhmatyoBuuttiServer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class GamesController : ControllerBase
    {
        private IUserRepository UserRepository;
        public GamesController(IUserRepository iUserRepository)
        {
            UserRepository = iUserRepository;
        }

        [HttpPatch("addsteamid")]
        public IActionResult AddSteamIdToUser(long id, JsonPatchDocument<UserSteamIdDTO> userUpdates)
        {
            User user = UserRepository.findUser(id);
            UserSteamIdDTO steamIdDTO = new UserSteamIdDTO
            { SteamId = user.SteamId };
            userUpdates.ApplyTo(steamIdDTO);

            user.SteamId = steamIdDTO.SteamId;
            UserRepository.UpdateUser(user);

            return Ok(new { message = "Steam ID added to user." });
        }
    }
}
