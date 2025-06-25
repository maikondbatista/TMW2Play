using Microsoft.AspNetCore.Mvc;
using TMW2Play.Api.Controllers.Base;
using TMW2Play.Infra.HTTP.Steam;
using TMW2Play.Service.Domain.Services;
namespace TMW2Play.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController(ISteamHttpService _steamService, INotificationService notificationService) : ApiController(notificationService)
    {
        [HttpGet("owned-games/{steamId}")]
        public async Task<IActionResult> OwnedGames([FromRoute] string steamId, CancellationToken cancellationToken)
        {
            var games = await _steamService.GetOwnedGames(steamId, cancellationToken);
            return await ResponseAsync(games);
        }

        [HttpGet("player-summary/{steamId}")]
        public async Task<IActionResult> PlayerSummary([FromRoute] string steamId, CancellationToken cancellationToken)
        {
            var summary = await _steamService.GetPlayerSummary(steamId, cancellationToken);
            return await ResponseAsync(summary);
        }

        [HttpGet("steam-user-id/{username}")]
        public async Task<IActionResult> SteamUserId([FromRoute] string username, CancellationToken cancellationToken)
        {
            var userId = await _steamService.GetSteamUserId(username, cancellationToken);
            return await ResponseAsync(userId);
        }
    }

}
