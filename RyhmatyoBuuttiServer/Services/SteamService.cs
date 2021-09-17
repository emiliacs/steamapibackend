namespace RyhmatyoBuuttiServer.Services
{
    public interface ISteamService
    {
        string PlayerSummariesUrl(string apiKey, string steamId);
        string PlayerOwnedGamesUrl(string apiKey, string steamId);
    }

    public class SteamService : ISteamService
    {
        public string PlayerSummariesUrl(string apiKey, string steamId)
        {
            return "https://" + $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={apiKey}&steamids={steamId}";
        }

        public string PlayerOwnedGamesUrl(string apiKey, string steamId)
        {
            return "https://" + $"api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={apiKey}&steamid={steamId}&include_appinfo=1";
        }
    }
}
