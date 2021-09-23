namespace RyhmatyoBuuttiServer
{
    public static class Constants
    {
        public static string PlayerSummariesUrl(string apiKey, string steamId)
        {
            return "https://" + $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={apiKey}&steamids={steamId}";
        }

        public static string PlayerOwnedGamesUrl(string apiKey, string steamId)
        {
            return "https://" + $"api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={apiKey}&steamid={steamId}&include_appinfo=1";
        }
        public static string GameExtraDataUrl(int appId)
        {
            return "https://" + $"store.steampowered.com/api/appdetails/?appids={appId}"; 
        }
    }
}
