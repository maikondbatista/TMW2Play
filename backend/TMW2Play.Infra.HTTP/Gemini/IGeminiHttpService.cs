using TMW2Play.Domain.Entities.Gemini.JsonPrompt;
using TMW2Play.Domain.Entities.Gemini.Response;

namespace TMW2Play.Infra.HTTP.Gemini
{
    public interface IGeminiHttpService
    {
        Task<Part?> UserResume(List<string> GamesWPlayTime, string language, CancellationToken cancellationToken = default);
        Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default);
    }
}
