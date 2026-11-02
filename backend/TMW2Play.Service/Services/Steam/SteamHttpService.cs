using Microsoft.Extensions.Caching.Hybrid;
using TMW2Play.Domain.Core.Steam;
using TMW2Play.Domain.Entities.Steam;
using TMW2Play.Domain.Interfaces.Services;

namespace TMW2Play.Infra.HTTP.Steam
{
    public class SteamHttpService(IHttpService httpService, SteamApiConfiguration steamApiService, HybridCache cache) : ISteamHttpService
    {
        public async Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default)
        {
            var url = steamApiService.UserIdUrl(username);
            return await cache.GetOrCreateAsync($"{username}-GetSteamUserId",
                async cancel => await httpService.GetAsync<SteamApiResponse<SteamUserResponse>>(url, cancel),
                tags: ["Steam"], cancellationToken: cancellationToken);
        }

        public async Task<SteamApiResponse<PlayerSummaryResponse>> GetPlayerSummary(string steamId, CancellationToken cancellationToken = default)
        {
            var url = steamApiService.PlayerSummaryUrl(steamId);
            return await cache.GetOrCreateAsync($"{steamId}-GetPlayerSummary",
                async cancel => await httpService.GetAsync<SteamApiResponse<PlayerSummaryResponse>>(url, cancel),
                tags: ["Steam"], cancellationToken: cancellationToken);
        }

        public async Task<SteamApiResponse<OwnedGamesResponse>> GetOwnedGames(string steamId, CancellationToken cancellationToken = default)
        {
            var url = steamApiService.OwnedGamesUrl(steamId);
            var response = await cache.GetOrCreateAsync($"{steamId}-GetOwnedGames",
                async cancel => await httpService.GetAsync<SteamApiResponse<OwnedGamesResponse>>(url, cancel),
                tags: ["Steam"], cancellationToken: cancellationToken);
            if (response?.Response?.games is null) return default;
            response.Response.games = response.Response.games.OrderByDescending(s => s?.playtime_forever).ToList();
            return response;
        }
    }
}
