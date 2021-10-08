using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ResetCode { get; set; }
        public DateTime? ResetCodeExpires { get; set; }
        public bool Verified { get; set; } = false;
        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeExpires { get; set; }
        public string SteamId { get; set; }
        public string RecentlyPlayedGame { get; set; }
        public int RecentlyPlayedMinutes { get; set; }
        public ICollection<FriendDTO> Friends { get; set; }
    }
}
