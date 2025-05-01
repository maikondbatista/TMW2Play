using TMW2Play.Domain.Entities.Steam.Response;

namespace TMW2Play.Infra.HTTP.Steam
{
    public interface ISteamHttpService
    {
        Task<OwnedGamesModel> GetUserOwnedGames(string steamId, CancellationToken cancellationToken = default);
    }
}