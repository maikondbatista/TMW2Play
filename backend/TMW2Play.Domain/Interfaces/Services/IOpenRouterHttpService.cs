using TMW2Play.Domain.DTO;
using TMW2Play.Domain.Entities.OpenRouter.JsonPrompt;

namespace TMW2Play.Domain.Interfaces.Services
{
    public interface IOpenRouterHttpService
    {
        Task<string> HumiliateMyLibrary(HumiliateMyLibraryRequest request, string language, CancellationToken cancellationToken = default);
        Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default);
        Task<List<GameUpcoming>> TellMeWhatIsUpcoming(TellMeWhatIsUpcomingRequest request, string language, CancellationToken cancellationToken);
    }
}
