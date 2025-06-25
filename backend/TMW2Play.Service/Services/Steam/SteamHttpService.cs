using Microsoft.Extensions.Caching.Hybrid;
using TMW2Play.Domain.Core.Steam;
using TMW2Play.Domain.Entities.Steam;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Infra.HTTP.Steam
{
    public class SteamHttpService(IHttpService httpService, SteamApiConfiguration steamApiService, INotificationService notificationService, HybridCache cache) : ISteamHttpService
    {
        public async Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default)
        {
            try
            {
                var urlUserId = steamApiService.UserIdUrl(username);
                var response = await cache.GetOrCreateAsync($"{username}-GetSteamUserId", async (cancel) => await httpService.GetAsync<SteamApiResponse<SteamUserResponse>>(urlUserId, cancel), tags: ["Steam"], cancellationToken: cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                notificationService.AddNotification($"An error occurred while fetching Steam user ID.");
            }
            return default;
        }

        public async Task<SteamApiResponse<PlayerSummaryResponse>> GetPlayerSummary(string steamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var urlUserId = steamApiService.PlayerSummaryUrl(steamId);
                var response = await cache.GetOrCreateAsync($"{steamId}-GetPlayerSummary", async (cancel) => await httpService.GetAsync<SteamApiResponse<PlayerSummaryResponse>>(urlUserId, cancel), tags: ["Steam"], cancellationToken: cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                notificationService.AddNotification($"An error occurred while fetching player summary.");
            }
            return default;
        }

        public async Task<SteamApiResponse<OwnedGamesResponse>> GetOwnedGames(string steamId, CancellationToken cancellationToken = default)
        {
            try
            {
                var userOwnedGamesUrl = steamApiService.OwnedGamesUrl(steamId);
                var response = await cache.GetOrCreateAsync($"{steamId}-GetOwnedGames", async (cancel) => await httpService.GetAsync<SteamApiResponse<OwnedGamesResponse>>(userOwnedGamesUrl, cancel), tags: ["Steam"], cancellationToken: cancellationToken);
                if (response != null && response.Response?.games != null)
                {
                    response.Response.games = response.Response.games
                        .OrderByDescending(s => s?.playtime_forever)
                        .ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                notificationService.AddNotification($"An error occurred while fetching owned games.");
            }
            return default;
        }
    }
}
