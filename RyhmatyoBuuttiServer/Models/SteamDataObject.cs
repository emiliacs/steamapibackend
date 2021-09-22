namespace RyhmatyoBuuttiServer.Models
{
    public class SteamDataObject
    {
        public Response response { get; set; }
    }

    public class Response
    {
        public SteamPlayer[] players { get; set; }
        public SteamGame[] games { get; set; }
    }

    public class SteamPlayer
    {
        public string personaname { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
    }

    public class SteamGame
    {
        public int appid { get; set; }
        public string name { get; set; }
        public int playtime_forever { get; set; }
    }
}
