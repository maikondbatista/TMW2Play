
using System.Net.Http.Json;
using System.Text.Json;
using TMW2Play.Domain.Core.Gemini;
using TMW2Play.Domain.Entities.Gemini.JsonPrompt;
using TMW2Play.Domain.Entities.Gemini.Response;
using TMW2Play.Domain.Entities.Steam;

namespace TMW2Play.Infra.HTTP.Gemini
{
    public class GeminiHttpService(HttpClient httpClient, GeminiApiConfiguration geminiApiService) : IGeminiHttpService
    {
        /// <summary>
        /// Returns the ML comment from user games
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="GamesWPlayTime">Games with their playtime, example: dota 2 - 100, cyberpunk 2077 - 100</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Part?> UserResume(List<string> GamesWPlayTime, string language, CancellationToken cancellationToken = default)
        {
            //CACHE
            var generateGamerCommentUrl = geminiApiService.LLMUrl();
            var body = geminiApiService.GamerCommentBody(GamesWPlayTime, language);
            var response = await httpClient.PostAsJsonAsync(generateGamerCommentUrl, body, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<GeminiApiResponse>(cancellationToken);
                return content?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault() ?? default;
            }
            return default;
        }

        public async Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default)
        {
            var generateGamerCommentUrl = geminiApiService.LLMUrl();
            var body = geminiApiService.TellMeWhatToPlayBody(GamesWPlayTime, allGames, language);
            if (body != null)
            {
                var response = await httpClient.PostAsJsonAsync(generateGamerCommentUrl, body, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<GeminiApiResponse>(cancellationToken);
                    var jsonRecommendation = content?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault().Text ?? default;

                    // Remove all backticks from the JSON string before deserialization
                    if (!string.IsNullOrEmpty(jsonRecommendation))
                    {
                        jsonRecommendation = jsonRecommendation.Replace("`", string.Empty);
                        jsonRecommendation = jsonRecommendation.Replace("json", string.Empty);
                    }

                    var recommendations = JsonSerializer.Deserialize<List<GameRecommendation>>(jsonRecommendation.ToString());
                    return recommendations;
                }
            }
            return default;
        }
    }
}
