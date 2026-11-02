using System.Text;
using System.Text.Json;

namespace TMW2Play.Api.ErrorReporting;

public sealed class DiscordErrorReporter(HttpClient httpClient, IConfiguration configuration,
    IHttpContextAccessor contextAccessor, IWebHostEnvironment environment) : IDisposable
{
    private readonly string? _webhookUrl = configuration["DiscordWebHookURL"];
#if DEBUG
    private const string BuildMode = "Debug";
#else
    private const string BuildMode = "Release";
#endif

    public bool IsEnabled => !string.IsNullOrWhiteSpace(_webhookUrl);

    public void Notify(Exception exception)
    {
        if (!IsEnabled) return;

        var context = contextAccessor.HttpContext;
        var header = $"TMW2Play | [{BuildMode}] | {environment.EnvironmentName} | {DateTime.UtcNow:O}\n"
            + $"Endpoint: {context?.Request.Method} {context?.Request.Path}\n";

        // No await here cause i want the webhook is 'fire-and-forget'.
        _ = SendAsync(header, exception.ToString());
    }

    private async Task SendAsync(string header, string diagnostic)
    {
        try
        {
            var attach = header.Length + diagnostic.Length > 1800;
            var payload = JsonSerializer.Serialize(new
            {
                content = header + (attach ? "attachment" : diagnostic),
                allowed_mentions = new { parse = Array.Empty<string>() }
            });

            using HttpContent content = attach ? CreateAttachment(payload, diagnostic)
                : new StringContent(payload, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(_webhookUrl, content);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            // Webhook error must not affect the API
        }
    }

    private static MultipartFormDataContent CreateAttachment(string payload, string diagnostic)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(payload, Encoding.UTF8), "payload_json");
        content.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(diagnostic)), "files[0]", "exception.txt");
        return content;
    }

    public void Dispose() => httpClient.Dispose();
}
