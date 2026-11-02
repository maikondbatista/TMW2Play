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
            var body = openRouterConfig.HumiliateMyLibrary(request, language);
            if (body is null)
            {
                notification.AddNotification("No games added to library.");
                return null;
            }

            var response = await httpService.PostAsync<OpenRouterApiResponse>(openRouterConfig.LLMUrl(), body, cancellationToken, OpenRouterAuthHeaders());
            var comment = response?.Choices?.FirstOrDefault()?.Message?.Content;
            if (string.IsNullOrWhiteSpace(comment))
                throw new InvalidDataException("OpenRouter returned an empty humiliation response.");
            return CleanJson(comment);
        }

        public async Task<List<GameRecommendation>> TellMeWhatToPlay(List<string> GamesWPlayTime, List<string> allGames, string language, CancellationToken cancellationToken = default)
        {
            var body = openRouterConfig.TellMeWhatToPlayBody(GamesWPlayTime, allGames, language);
            if (body is null)
            {
                notification.AddNotification("No games added to library.");
                return [];
            }

            var response = await httpService.PostAsync<OpenRouterApiResponse>(openRouterConfig.LLMUrl(), body, cancellationToken, OpenRouterAuthHeaders());
            var content = response?.Choices?.FirstOrDefault()?.Message?.Content;
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidDataException("OpenRouter returned an empty recommendations response.");

            var jsonArray = ParseJsonArray(CleanJson(content));
            var schema = JsonSchema.FromText(openRouterConfig.TMW2PlaySchema);
            foreach (var item in jsonArray)
            {
                var result = schema.Evaluate(JsonSerializer.SerializeToElement(item));
                if (!result.IsValid)
                    throw new JsonException("OpenRouter recommendations do not match the required schema: " +
                        string.Join("; ", result.Details?.Select(d => d.ToString()) ?? []));
            }

            return jsonArray.Deserialize<List<GameRecommendation>>()
                ?? throw new JsonException("Failed to deserialize OpenRouter recommendations.");
        }

        public async Task<List<GameUpcoming>> TellMeWhatIsUpcoming(TellMeWhatIsUpcomingRequest request, string language, CancellationToken cancellationToken)
        {
            var body = openRouterConfig.TellMeWhatIsUpcomingBody(request.LastTwoWeeks, request.AllGames, language);
            if (body is null)
            {
                notification.AddNotification("No games added to library.");
                return [];
            }

            var response = await httpService.PostAsync<OpenRouterApiResponse>(openRouterConfig.LLMUrl(), body, cancellationToken, OpenRouterAuthHeaders());
            var content = response?.Choices?.FirstOrDefault()?.Message?.Content;
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidDataException("OpenRouter returned an empty upcoming games response.");

            var jsonArray = ParseJsonArray(CleanJson(content));
            var schema = JsonSchema.FromText(openRouterConfig.UpcomingGameReleaseSchema);
            foreach (var item in jsonArray)
            {
                var result = schema.Evaluate(JsonSerializer.SerializeToElement(item));
                if (!result.IsValid)
                    throw new JsonException("OpenRouter upcoming games do not match the required schema: " +
                        string.Join("; ", result.Details?.Select(d => d.ToString()) ?? []));
            }

            return jsonArray.Deserialize<List<GameUpcoming>>()
                ?? throw new JsonException("Failed to deserialize OpenRouter upcoming games.");
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

        private static JsonArray ParseJsonArray(string json)
        {
            var node = JsonNode.Parse(json);
            if (node is JsonArray array) return array;

            if (node is JsonObject obj)
            {
                foreach (var key in new[] { "recommendations", "games", "all_games", "data", "results" })
                    if (obj[key] is JsonArray nestedArray) return nestedArray;
            }

            throw new JsonException("Expected a JSON array or an object containing a recommendations array.");
        }
    }
}
