using System;

namespace RyhmatyoBuuttiServer.Models
{
    public class PasswordResetCode
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
