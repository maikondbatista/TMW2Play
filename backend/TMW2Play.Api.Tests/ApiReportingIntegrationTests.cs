using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TMW2Play.Api;
using TMW2Play.Api.ErrorReporting;
using TMW2Play.Domain.Interfaces.Services;

namespace TMW2Play.Api.Tests;

public sealed class ApiReportingIntegrationTests
{
    [Fact]
    public async Task Production_response_contains_no_private_url_exception_details_or_redirect()
    {
        const string webhook = "https://discord.com/api/webhooks/123/TEST_WEBHOOK_SECRET";
        var transport = new RecordingTransport();
        using var server = CreateServer(webhook, transport, "Production");
        using var client = server.CreateClient();

        var response = await client.GetAsync("/__test/errors/exception?token=TEST_QUERY_SECRET");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Null(response.Headers.Location);
        Assert.Equal(new[] { "An unexpected error occurred. Please try again later." },
            await response.Content.ReadFromJsonAsync<string[]>());

        var publicData = response.Headers.ToString() + response.Content.Headers.ToString() + await response.Content.ReadAsStringAsync();
        foreach (var secret in new[] { webhook, "TEST_WEBHOOK_SECRET", "TEST_QUERY_SECRET", "TEST_EXCEPTION_SECRET", "private.example.invalid", "private-source.cs", "InvalidOperationException" })
            Assert.DoesNotContain(secret, publicData);

        await transport.Sent.Task.WaitAsync(TimeSpan.FromSeconds(3));
        var diagnostic = transport.Attachment is null ? transport.Payload : System.Text.Encoding.UTF8.GetString(transport.Attachment);
        Assert.Contains("Endpoint: GET /__test/errors/exception", transport.Payload);
        Assert.Contains("Route: ", transport.Payload);
        Assert.Contains("TEST_EXCEPTION_SECRET", diagnostic);
        Assert.Contains("private-source.cs", diagnostic);
    }

    [Fact]
    public async Task Production_does_not_publish_error_swagger_or_configuration_urls()
    {
        var root = Directory.CreateTempSubdirectory("TMW2Play-ProductionAudit-").FullName;
        var webRoot = Path.Combine(root, "wwwroot");
        Directory.CreateDirectory(webRoot);
        var privateFiles = new[] { "appsettings.json", "appsettings.Production.json", "appsettings.example.json", "exception.txt" };
        foreach (var name in privateFiles)
            await File.WriteAllTextAsync(Path.Combine(root, name), "PRIVATE_CONFIGURATION_OR_EXCEPTION_DATA");

        try
        {
            using var server = CreateServer(null, environment: "Production", contentRoot: root);
            using var client = server.CreateClient();
            var paths = new[] { "/error", "/exception", "/api/telemetry/errors", "/api/telemetry/capabilities", "/swagger/index.html", "/swagger/v1/swagger.json" }
                .Concat(privateFiles.Select(name => "/" + name));
            foreach (var path in paths)
            {
                var response = await client.GetAsync(path);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.Null(response.Headers.Location);
                Assert.DoesNotContain("PRIVATE_CONFIGURATION_OR_EXCEPTION_DATA", await response.Content.ReadAsStringAsync());
            }
        }
        finally
        {
            foreach (var name in privateFiles) File.Delete(Path.Combine(root, name));
            Directory.Delete(webRoot);
            Directory.Delete(root);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Missing_or_empty_webhook_does_not_break_api_or_attempt_delivery(string? webhookUrl)
    {
        var transport = new RecordingTransport();
        using var server = CreateServer(webhookUrl, transport);
        using var client = server.CreateClient();

        var success = await client.GetAsync("/__test/errors/success");
        Assert.Equal(HttpStatusCode.OK, success.StatusCode);

        var failure = await client.GetAsync("/__test/errors/exception");
        Assert.Equal(HttpStatusCode.InternalServerError, failure.StatusCode);
        var body = await failure.Content.ReadAsStringAsync();
        Assert.Contains("An unexpected error occurred", body);
        Assert.DoesNotContain("original test failure", body);
        Assert.False(server.Services.GetRequiredService<DiscordErrorReporter>().IsEnabled);
        Assert.Equal(0, transport.Attempts);
    }

    [Fact]
    public async Task Disabled_api_has_no_reporting_routes()
    {
        using var server = CreateServer(null);
        using var client = server.CreateClient();

        Assert.False(server.Services.GetRequiredService<DiscordErrorReporter>().IsEnabled);
        var capability = await client.GetAsync("/api/telemetry/capabilities");
        Assert.Equal(HttpStatusCode.NotFound, capability.StatusCode);
        using var invalidJson = new StringContent("{ not json", System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/telemetry/errors", invalidJson);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Enabled_api_does_not_expose_notification_endpoint()
    {
        var transport = new RecordingTransport();
        using var server = CreateServer("https://discord.com/api/webhooks/123/test_token", transport);
        using var client = server.CreateClient();

        var response = await client.PostAsync("/api/telemetry/errors", new StringContent("{}"));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(response.Headers.Contains("X-Error-Event-Id"));
        Assert.Equal(0, transport.Attempts);
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Production")]
    public async Task Exception_handler_hides_exception_from_user_and_reports_original(string environment)
    {
        var transport = new RecordingTransport();
        using var server = CreateServer("https://discord.com/api/webhooks/123/test_token", transport, environment);
        using var client = server.CreateClient();
        var response = await client.GetAsync("/__test/errors/exception");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var userMessage = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("original test failure", userMessage);
        Assert.DoesNotContain("InvalidOperationException", userMessage);
        Assert.Contains("An unexpected error occurred", userMessage);
        await transport.Sent.Task.WaitAsync(TimeSpan.FromSeconds(3));
        var diagnostic = transport.Attachment is null ? transport.Payload : System.Text.Encoding.UTF8.GetString(transport.Attachment);
        Assert.Contains("InvalidOperationException", diagnostic);
        Assert.Contains("original test failure", diagnostic);
        Assert.Equal(1, transport.Attempts);
    }

    [Fact]
    public async Task Dependency_error_reaches_handler_without_becoming_a_user_notification()
    {
        var transport = new RecordingTransport();
        using var server = CreateServer("https://discord.com/api/webhooks/123/test_token", transport);
        using var client = server.CreateClient();
        var response = await client.GetAsync("/__test/errors/dependency");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("An unexpected error occurred", body);
        Assert.DoesNotContain("original dependency failure", body);
        Assert.DoesNotContain("HttpRequestException", body);
        await transport.Sent.Task.WaitAsync(TimeSpan.FromSeconds(3));
        var diagnostic = transport.Attachment is null ? transport.Payload : System.Text.Encoding.UTF8.GetString(transport.Attachment);
        Assert.Contains("HttpRequestException", diagnostic);
        Assert.Contains("original dependency failure", diagnostic);
        Assert.Contains("503", diagnostic);
        Assert.Equal(1, transport.Attempts);
    }

    [Fact]
    public async Task Automatic_model_validation_keeps_concrete_reason()
    {
        var transport = new RecordingTransport();
        using var server = CreateServer("https://discord.com/api/webhooks/123/test_token", transport);
        using var client = server.CreateClient();
        var response = await client.PostAsync("/__test/errors/validation", new StringContent("{}", System.Text.Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Steam ID is required", await response.Content.ReadAsStringAsync());
        Assert.Equal(0, transport.Attempts);
    }

    private static TestServer CreateServer(string? webhookUrl, RecordingTransport? transport = null, string environment = "Production", string? contentRoot = null)
    {
        var builder = new WebHostBuilder()
            .UseEnvironment(environment)
            .UseSetting("https_port", "")
            .ConfigureAppConfiguration(config => config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DiscordWebHookURL"] = webhookUrl,
                ["SteamAPIKey"] = "test-steam-key",
                ["OpenRouterAPIKey"] = "test-openrouter-key",
                ["OpenRouterModel"] = "test-model"
            }))
            .ConfigureTestServices(services =>
            {
                services.AddControllers().AddApplicationPart(typeof(TestErrorsController).Assembly);
                services.AddSingleton(new HttpClient(new DependencyFailureHandler()));
                if (transport is not null)
                {
                    services.RemoveAll<DiscordErrorReporter>();
                    services.AddSingleton(sp => new DiscordErrorReporter(new HttpClient(transport),
                        sp.GetRequiredService<IConfiguration>(), sp.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                        sp.GetRequiredService<IWebHostEnvironment>()));
                }
            })
            .UseStartup<Startup>();
        if (contentRoot is not null) builder.UseContentRoot(contentRoot).UseWebRoot("wwwroot");
        return new TestServer(builder);
    }


    private sealed class DependencyFailureHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable) { ReasonPhrase = "original dependency failure" });
    }

    private sealed class RecordingTransport : HttpMessageHandler
    {
        public int Attempts { get; private set; }
        public string Payload { get; private set; } = string.Empty;
        public byte[]? Attachment { get; private set; }
        public TaskCompletionSource Sent { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Attempts++;
            if (request.Content is MultipartContent multipart)
            {
                foreach (var part in multipart)
                {
                    if (part.Headers.ContentDisposition?.Name?.Trim('"') == "payload_json") Payload = await part.ReadAsStringAsync(cancellationToken);
                    else Attachment = await part.ReadAsByteArrayAsync(cancellationToken);
                }
            }
            else Payload = await request.Content!.ReadAsStringAsync(cancellationToken);
            Sent.TrySetResult();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}

// Registered only in TestServer; this assembly is never deployed with the API.
[ApiController]
[Route("__test/errors")]
public sealed class TestErrorsController : ControllerBase
{
    [HttpGet("success")]
    public IActionResult Success() => Ok();

    [HttpGet("exception")]
    public IActionResult Throw() => throw new InvalidOperationException("original test failure: https://private.example.invalid/debug?token=TEST_EXCEPTION_SECRET | private-source.cs:line 42");

    [HttpGet("dependency")]
    public async Task<IActionResult> Dependency([FromServices] IHttpService service)
    {
        await service.GetAsync<object>("https://dependency.invalid", CancellationToken.None);
        return Ok();
    }

    [HttpPost("validation")]
    public IActionResult Validate(TestErrorRequest request) => Ok();
}

public sealed class TestErrorRequest
{
    [Required(ErrorMessage = "Steam ID is required")]
    public string? SteamId { get; init; }
}
