namespace RyhmatyoBuuttiServer.Models
{
    public class UserAuthenticateResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
    }
}
