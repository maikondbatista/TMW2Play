using System.Net.Http.Json;
using TMW2Play.Domain.Interfaces.Services;

namespace TMW2Play.Service.Services.Http
{
    public class HttpService(HttpClient httpClient) : IHttpService
    {
        public async Task<TResponse?> PostAsync<TResponse>(string url, object body, CancellationToken cancellationToken, Dictionary<string, string>? headers = null)
        {
            ApplyHeaders(headers);
            using var response = await httpClient.PostAsJsonAsync(url, body, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url, CancellationToken cancellationToken, Dictionary<string, string>? headers = null)
        {
            ApplyHeaders(headers);
            using var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
        }

        private void ApplyHeaders(Dictionary<string, string>? headers)
        {
            if (headers is null) return;
            foreach (var header in headers)
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
}
