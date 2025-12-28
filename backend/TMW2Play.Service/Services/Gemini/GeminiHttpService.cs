using Json.Schema;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;
using System.Text.Json.Nodes;
using TMW2Play.Domain.Core.Gemini;
using TMW2Play.Domain.Entities.Gemini.JsonPrompt;
using TMW2Play.Domain.Entities.Gemini.Response;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Infra.HTTP.Gemini
{
    public class GeminiHttpService(IHttpService httpService, GeminiApiConfiguration geminiConfig, INotificationService notification) : IGeminiHttpService
    {
        /// <summary>
        /// Returns the ML comment from user games.
        /// </summary>
        public async Task<PartResponse?> UserResume(List<string> GamesWPlayTime, string language, CancellationToken cancellationToken = default)
        {
            try
            {
                //Add steamID on request headers, read and cache.
                var generateGamerCommentUrl = geminiConfig.LLMUrl();
                var body = geminiConfig.GamerCommentBody(GamesWPlayTime, language);
                var response = await httpService.PostAsync<GeminiApiResponse>(generateGamerCommentUrl, body, cancellationToken, GeminiAuthHeaders());
                return response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault();
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while fetching user resume.");
            }
            return default;
        }

        /// <summary>
        /// Returns a list of recommended games based on user's playtime and library.
        /// </summary>
        public async Task<List<GameRecommendation>> TellMeWhatToPlay(
     List<string> GamesWPlayTime,
     List<string> allGames,
     string language,
     CancellationToken cancellationToken = default)
        {
            try
            {
                var generateGamerCommentUrl = geminiConfig.LLMUrl();
                var body = geminiConfig.TellMeWhatToPlayBody(GamesWPlayTime, allGames, language);

                if (body is null)
                {
                    notification.AddNotification("No games added to library.");
                    return [];
                }
                var response = await httpService.PostAsync<GeminiApiResponse>(generateGamerCommentUrl, body, cancellationToken, GeminiAuthHeaders());
                var jsonRecommendation = response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

                if (string.IsNullOrWhiteSpace(jsonRecommendation))
                {
                    notification.AddNotification("No recommendations found in AI response.");
                    return [];
                }

                var cleanedJson = CleanJson(jsonRecommendation);

                if (!TryParseJsonArray(cleanedJson, out var jsonArray, out var parseError))
                {
                    notification.AddNotification($"Invalid JSON format in recommended games response: {parseError}");
                    return [];
                }

                var schema = JsonSchema.FromText(geminiConfig.TMW2PlaySchema);

                foreach (var item in jsonArray)
                {
                    var jsonElement = JsonSerializer.SerializeToElement(item);
                    var result = schema.Evaluate(jsonElement);
                    if (!result.IsValid)
                    {
                        notification.AddNotification("One or more recommendations do not match the required schema: " +
                            string.Join("; ", result.Details?.Select(d => d) ?? []));
                        return [];
                    }
                }

                try
                {
                    var recommendations = jsonArray.Deserialize<List<GameRecommendation>>();
                    if (recommendations is not null)
                        return recommendations;

                    notification.AddNotification("Failed to parse recommended games response.");
                }
                catch (JsonException ex)
                {
                    notification.AddNotification($"Deserialization error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                notification.AddNotification($"An error occurred while fetching recommended games: {ex.Message}");
            }
            return [];
        }

        private Dictionary<string, string> GeminiAuthHeaders()
        {
            return new Dictionary<string, string>
                {
                    { "X-goog-api-key", geminiConfig.LLMApiKey }
                };
        }

        private static string CleanJson(string input) =>
            input.Replace("`", string.Empty).Replace("json", string.Empty);

        private static bool TryParseJsonArray(string json, out JsonArray jsonArray, out string error)
        {
            try
            {
                var node = JsonNode.Parse(json);
                if (node is JsonArray arr)
                {
                    jsonArray = arr;
                    error = string.Empty;
                    return true;
                }

                if (node is JsonObject obj && obj["recommendations"] is JsonArray recommendationsArray)
                {
                    jsonArray = recommendationsArray;
                    error = string.Empty;
                    return true;
                }

                jsonArray = new JsonArray();
                error = "Expected either a JSON array or an object with 'recommendations' array.";
                return false;
            }
            catch (JsonException ex)
            {
                jsonArray = new JsonArray();
                error = ex.Message;
                return false;
            }
        }
    }
}
