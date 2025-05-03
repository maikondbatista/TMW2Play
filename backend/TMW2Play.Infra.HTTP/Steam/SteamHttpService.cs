using System.Net.Http.Json;
using TMW2Play.Domain.Core.Steam;
using TMW2Play.Domain.Entities.Steam;

namespace TMW2Play.Infra.HTTP.Steam
{
    public class SteamHttpService(HttpClient httpClient, SteamApiConfiguration steamApiService) : ISteamHttpService
    {
        public async Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default)
        {

            var urlUserId = steamApiService.UserIdUrl(username);
            var response = await httpClient.GetAsync(urlUserId, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SteamApiResponse<SteamUserResponse>>(cancellationToken);
                return content;
            }
            return default;
        }
        public async Task<SteamApiResponse<PlayerSummaryResponse>> GetPlayerSummary(string steamId, CancellationToken cancellationToken = default)
        {
            var urlUserId = steamApiService.PlayerSummaryUrl(steamId);
            var response = await httpClient.GetAsync(urlUserId, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SteamApiResponse<PlayerSummaryResponse>>(cancellationToken);
                return content;
            }
            return default;
        }
        public async Task<SteamApiResponse<OwnedGamesResponse>> GetOwnedGames(string steamId, CancellationToken cancellationToken = default)
        {
            var userOwnedGamesUrl = steamApiService.OwnedGamesUrl(steamId);
            var response = await httpClient.GetAsync(userOwnedGamesUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SteamApiResponse<OwnedGamesResponse>>(cancellationToken);
                return content;
            }
            return default;
        }
    }
}
