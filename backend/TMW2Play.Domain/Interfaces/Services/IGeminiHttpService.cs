using TMW2Play.Domain.DTO;
using TMW2Play.Domain.Entities.Gemini.JsonPrompt;
using TMW2Play.Domain.Entities.Gemini.Response;

namespace TMW2Play.Infra.HTTP.Gemini
{
    public interface IGeminiHttpService
    {
        Task<string> HumiliateMyLibrary(HumiliateMyLibraryRequest request, string language, CancellationToken cancellationToken = default);
        Task<List<GameUpcoming>> TellMeWhatIsUpcoming(TellMeWhatIsUpcomingRequest request, string language, CancellationToken cancellationToken);
        Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default);
    }
}
