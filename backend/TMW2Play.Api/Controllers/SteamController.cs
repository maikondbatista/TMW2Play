using Microsoft.AspNetCore.Mvc;
using TMW2Play.Infra.HTTP.Steam;

namespace TMW2Play.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController(ILogger<SteamController> _logger, ISteamHttpService _steamService) : ControllerBase
    {
        [HttpGet(Name = "user-owned-games")]
        public async Task<IActionResult> Get(string steamId)
        {
            var games = await _steamService.GetUserOwnedGames(steamId);
            return Ok(games);
        }
    }
}
