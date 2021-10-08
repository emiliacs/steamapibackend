using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public long UserEntityId { get; set; }
        public long FriendEntityId { get; set; }
        public string FriendName { get; set; }
        public string RecentlyPlayedGame { get; set; }
        public int RecentlyPlayedMinutes { get; set; }
    }
}

