using System.Collections.Generic;

namespace RyhmatyoBuuttiServer.Models
{
    public class Game
    {
        public long Id { get; set; }
        public int SteamId { get; set; }
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string ImageUrl { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<Developer> Developers { get; set; }
        public List<Genre> Genres { get; set; }
    }
}


