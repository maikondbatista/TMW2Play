using System.Net.Http.Json;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Service.Services.Http
{
    public class HttpService(HttpClient httpClient, INotificationService notification) : IHttpService
    {

        public async Task<TResponse?> PostAsync<TResponse>(string url, object body, CancellationToken cancellationToken, Dictionary<string, string>? headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await httpClient.PostAsJsonAsync(url, body, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
                }
                notification.AddNotification($"Request failed. Status: {response.StatusCode}");
                return default;
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while processing the request.");
                return default;
            }
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url, CancellationToken cancellationToken, Dictionary<string, string>? headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await httpClient.GetAsync(url, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
                }
                notification.AddNotification($"Request failed. Status: {response.StatusCode}");
                return default;
            }
            catch (Exception)
            {
                notification.AddNotification("An error occurred while processing the request.");
                return default;
            }
        }

        private void ApplyHeaders(Dictionary<string, string>? headers)
        {
            if (headers == null || headers.Count == 0)
                return;

            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

    }
}
