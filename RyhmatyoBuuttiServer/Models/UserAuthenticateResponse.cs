using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RyhmatyoBuuttiServer.Models
{
    public class UserAuthenticateResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
    }
}
