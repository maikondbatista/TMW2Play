namespace TMW2Play.Domain.Core.Steam
{
    public class SteamApiConfiguration(string apiKey)
    {
        private string ApiKey { get; } = apiKey;
        private string SteamBaseUrl { get; } = "https://api.steampowered.com";
        private string SteamUser { get; } = "/ISteamUser";
        private string ResolveVanity { get; } = "/ResolveVanityURL";
        private string V1 { get; } = "/v1";

        public string UserIdByProfile(string profileName)
        {
            return $"{SteamBaseUrl}{SteamUser}{ResolveVanity}{V1}?vanityurl={profileName}&key={ApiKey}";
        }
    }
}   