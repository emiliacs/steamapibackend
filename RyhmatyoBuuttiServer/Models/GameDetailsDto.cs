using Newtonsoft.Json;
using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Models
{
    public class GameDetailsDto
    {
        [JsonProperty("release_date")]
        public GameReleaseDateDto ReleaseDate { get; set; }

        [JsonProperty("developers")]
        public List<string> Developers { get; }

        [JsonProperty("publishers")]
        public List<string> Publishers { get; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; }
        public GameDetailsDto()
        {
            this.Developers = new List<string>();
            this.Publishers = new List<string>();
            this.Genres = new List<Genre>();
 
        }

    }
}
