using System;

namespace RyhmatyoBuuttiServer.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ResetCode { get; set; }
        public DateTime? ResetCodeExpires { get; set; }
    }
}
