using Json.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using TMW2Play.Domain.Core.OpenRouter;
using TMW2Play.Domain.DTO;
using TMW2Play.Domain.Entities.OpenRouter.JsonPrompt;
using TMW2Play.Domain.Entities.OpenRouter.Response;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Service.Services.OpenRouter
{
    public class OpenRouterHttpService(IHttpService httpService, OpenRouterApiConfiguration openRouterConfig, INotificationService notification) : IOpenRouterHttpService
    {
        public async Task<string> HumiliateMyLibrary(HumiliateMyLibraryRequest request, string language, CancellationToken cancellationToken = default)
        {
            try
            {
                var url = openRouterConfig.LLMUrl();
                var body = openRouterConfig.HumiliateMyLibrary(request, language);

                if (body is null)
                {
                    notification.AddNotification("No games added to library.");
                    return null;
                }

                var response = await httpService.PostAsync<OpenRouterApiResponse>(url, body, cancellationToken, OpenRouterAuthHeaders());
                var comment = response?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(comment))
                {
                    notification.AddNotification("No Humiliation was able to be done.");
                    return null;
                }

                return CleanJson(comment);
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while fetching humiliation.");
            }
            return null;
        }

        public async Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default)
        {
            try
            {
                var url = openRouterConfig.LLMUrl();
                var body = openRouterConfig.TellMeWhatToPlayBody(GamesWPlayTime, allGames, language);

                if (body is null)
                {
                    notification.AddNotification("No games added to library.");
                    return [];
                }

                var response = await httpService.PostAsync<OpenRouterApiResponse>(url, body, cancellationToken, OpenRouterAuthHeaders());
                var jsonRecommendation = response?.Choices?.FirstOrDefault()?.Message?.Content;

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

                var schema = JsonSchema.FromText(openRouterConfig.TMW2PlaySchema);

                foreach (var item in jsonArray)
                {
                    var jsonElement = JsonSerializer.SerializeToElement(item);
                    var result = schema.Evaluate(jsonElement);
                    if (!result.IsValid)
                    {
                        notification.AddNotification("One or more recommendations do not match the required schema: " +
                            string.Join("; ", result.Details?.Select(d => d.ToString()) ?? []));
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
                catch (JsonException)
                {
                    notification.AddNotification("Deserialization error");
                }
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while fetching recommended games");
            }
            return [];
        }

        public async Task<List<GameUpcoming>> TellMeWhatIsUpcoming(TellMeWhatIsUpcomingRequest request, string language, CancellationToken cancellationToken)
        {
            try
            {
                var url = openRouterConfig.LLMUrl();
                var body = openRouterConfig.TellMeWhatIsUpcomingBody(request.LastTwoWeeks, request.AllGames, language);

                if (body is null)
                {
                    notification.AddNotification("No games added to library.");
                    return [];
                }

                var response = await httpService.PostAsync<OpenRouterApiResponse>(url, body, cancellationToken, OpenRouterAuthHeaders());
                var jsonUpcoming = response?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(jsonUpcoming))
                {
                    notification.AddNotification("No upcoming games found in AI response.");
                    return [];
                }

                var cleanedJson = CleanJson(jsonUpcoming);

                if (!TryParseJsonArray(cleanedJson, out var jsonArray, out var parseError))
                {
                    notification.AddNotification($"Invalid JSON format in upcoming games response: {parseError}");
                    return [];
                }

                var schema = JsonSchema.FromText(openRouterConfig.UpcomingGameReleaseSchema);
                foreach (var item in jsonArray)
                {
                    var jsonElement = JsonSerializer.SerializeToElement(item);
                    var result = schema.Evaluate(jsonElement);
                    if (!result.IsValid)
                    {
                        notification.AddNotification("One or more recommendations do not match the required schema: " +
                            string.Join("; ", result.Details?.Select(d => d.ToString()) ?? []));
                        return [];
                    }
                }

                try
                {
                    var gameReleases = jsonArray.Deserialize<List<GameUpcoming>>();
                    if (gameReleases is not null)
                        return gameReleases;

                    notification.AddNotification("Failed to parse recommended games response.");
                }
                catch (JsonException)
                {
                    notification.AddNotification("Deserialization error");
                }
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while fetching upcoming games.");
            }
            return [];
        }

        private Dictionary<string, string> OpenRouterAuthHeaders()
        {
            return new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {openRouterConfig.ApiKey}" },
                { "HTTP-Referer", "https://tmw2play.com.br" },
                { "X-Title", "TMW2Play" }
            };
        }

        private static string CleanJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (input.Contains("```json"))
                input = input.Split("```json")[1].Split("```")[0];
            else if (input.Contains("```"))
                input = input.Split("```")[1].Split("```")[0];

            // Fix misplaced comma inside array string: ["foo," "bar"] -> ["foo", "bar"]
            input = Regex.Replace(input, @"""([^""]*),(""\s*"")", "\"$1\", \"");

            return input.Trim();
        }

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

                if (node is JsonObject obj)
                {
                    var knownKeys = new[] { "recommendations", "games", "all_games", "data", "results" };
                    foreach (var key in knownKeys)
                    {
                        if (obj[key] is JsonArray nestedArray)
                        {
                            jsonArray = nestedArray;
                            error = string.Empty;
                            return true;
                        }
                    }
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
