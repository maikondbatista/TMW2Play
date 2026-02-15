using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TMW2Play.Api.Controllers.Base;
using TMW2Play.Domain.DTO;
using TMW2Play.Infra.HTTP.Gemini;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("AILimiter")]
    public class GeminiController(IGeminiHttpService geminiService, INotificationService notificationService) : ApiController(notificationService)
    {
        [HttpPost("tell-me-what-to-play")]
        public async Task<IActionResult> TellMeWhatToPlay([FromBody] TellMeWhatToPlayRequest request, CancellationToken cancellationToken)
        {
            var language = LoadUserLanguage();
            var games = await geminiService.TellMeWhatToPlay(request.LastTwoWeeks, request.AllGames, language, cancellationToken);
            return await ResponseAsync(games);
        }

        [HttpPost("humiliate-my-library")]
        public async Task<IActionResult> HumiliateMyLibrary([FromBody] HumiliateMyLibraryRequest request, CancellationToken cancellationToken)
        {
            var language = LoadUserLanguage();
            var games = await geminiService.HumiliateMyLibrary(request, language, cancellationToken);
            return await ResponseAsync(games);
        }

        [HttpPost("tell-me-what-is-upcoming")]
        public async Task<IActionResult> TellMeWhatIsUpcoming([FromBody] TellMeWhatIsUpcomingRequest request, CancellationToken cancellationToken)
        {
            var language = LoadUserLanguage();
            var games = await geminiService.TellMeWhatIsUpcoming(request, language, cancellationToken);
            return await ResponseAsync(games);
        }

        private string LoadUserLanguage()
        {
            // Use the AcceptLanguage property to access the 'Accept-Language' header
            var languageHeader = Request.Headers.AcceptLanguage.ToString() ?? "en-US";
            return languageHeader.Split(',')[0].Trim();
        }
    }
}
