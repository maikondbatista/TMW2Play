using System.Net.Http.Json;
using TMW2Play.Domain.Core.Steam;
using TMW2Play.Domain.Entities.Steam.Response;

namespace TMW2Play.Infra.HTTP.Steam
{
    public class SteamHttpService(HttpClient httpClient, SteamApiConfiguration steamApiService) : ISteamHttpService
    {
        public async Task<OwnedGamesModel> GetUserOwnedGames(string steamId, CancellationToken cancellationToken = default)
        {
            var urlUserId = steamApiService.UserIdByProfile(steamId);
            var response = await httpClient.GetAsync(urlUserId, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OwnedGamesModel>();
                return content;
            }
            else
            {
                throw new Exception($"Error fetching data from Steam API: {response.StatusCode}");
            }
        }
    }
}
