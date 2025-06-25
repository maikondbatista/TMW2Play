namespace TMW2Play.Domain.Core.Steam
{
    public class SteamApiConfiguration(string apiKey)
    {
        private string ApiKey { get; } = apiKey;
        private string SteamBaseUrl { get; } = "https://api.steampowered.com";
        private string SteamUser { get; } = "/ISteamUser";
        private string PlayerService { get; } = "/IPlayerService";
        private string ResolveVanity { get; } = "/ResolveVanityURL";
        private string GetOwnedGames { get; } = "/GetOwnedGames";
        private string GetPlayerSummaries { get; } = "/GetPlayerSummaries";
        private string V1 { get; } = "/v1";
        private string V2 { get; } = "/v2";

        private string AppendApiKey(string url)
        {
            return $"{url}&key={ApiKey}";
        }

        public string PlayerSummaryUrl(string steamId)
        {
            var url = $"{SteamBaseUrl + SteamUser + GetPlayerSummaries + V2}?steamids={steamId}";
            return AppendApiKey(url);
        }

        public string UserIdUrl(string profileName)
        {
            var url = $"{SteamBaseUrl + SteamUser + ResolveVanity + V1}?vanityurl={profileName}";
            return AppendApiKey(url);
        }

        public string OwnedGamesUrl(string steamId)
        {
            var url = $"{SteamBaseUrl + PlayerService + GetOwnedGames + V1}?steamid={steamId}&include_appinfo=1&include_played_free_games=1";
            return AppendApiKey(url);
        }
    }
}