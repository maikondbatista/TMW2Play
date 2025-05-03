using TMW2Play.Domain.Entities.Steam;

namespace TMW2Play.Infra.HTTP.Steam
{
    public interface ISteamHttpService
    {
        Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default);
        Task<SteamApiResponse<PlayerSummaryResponse>> GetPlayerSummary(string steamId, CancellationToken cancellationToken = default);
        Task<SteamApiResponse<OwnedGamesResponse>> GetOwnedGames(string steamId, CancellationToken cancellationToken = default);
    }
}