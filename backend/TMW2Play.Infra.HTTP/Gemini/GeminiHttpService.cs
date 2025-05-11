
using System.Net.Http.Json;
using TMW2Play.Domain.Core.Gemini;
using TMW2Play.Domain.Entities.Gemini.Response;
using TMW2Play.Domain.Entities.Steam;

namespace TMW2Play.Infra.HTTP.Gemini
{
        public class GeminiHttpService(HttpClient httpClient, GeminiApiConfiguration geminiApiService) : ISteamHttpService
        {
            public async Task<SteamApiResponse<SteamUserResponse>> GetSteamUserId(string username, CancellationToken cancellationToken = default)
            {

                var urlUserId = geminiApiService.GeminiPromptUrl(username);
                var response = await httpClient.GetAsync(urlUserId, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<GeminiApiResponse>(cancellationToken);
                    return content;
                }
                return default;
            }
        }
}
