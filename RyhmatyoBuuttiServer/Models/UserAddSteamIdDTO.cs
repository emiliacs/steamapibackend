using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserAddSteamIdDTO
    {
        [Required(ErrorMessage = "Steam ID not found.")]
        public string SteamId { get; set; }
    }
}
