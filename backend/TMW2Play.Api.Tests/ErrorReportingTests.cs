using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TMW2Play.Api.ErrorReporting;
using TMW2Play.Service.Domain.Services;
using TMW2Play.Service.Services.Notification;

namespace TMW2Play.Api.Tests;

public class ErrorReportingTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Missing_webhook_does_not_attempt_delivery(string? url)
    {
        var handler = new RecordingHandler();
        using var reporter = CreateReporter(handler, url);
        reporter.Notify(new Exception("original failure"));
        Assert.False(reporter.IsEnabled);
        Assert.Equal(0, handler.Attempts);
    }

    [Fact]
    public async Task Notification_contains_build_tag_and_original_exception()
    {
        var handler = new RecordingHandler();
        using var reporter = CreateReporter(handler);
        var exception = new InvalidOperationException("token=test-value", new ArgumentException("original inner error"));

        reporter.Notify(exception);
        await handler.Completed.Task.WaitAsync(TimeSpan.FromSeconds(3));

        var content = JsonDocument.Parse(handler.Payload).RootElement.GetProperty("content").GetString()!;
#if DEBUG
        Assert.Contains("[Debug]", content);
#else
        Assert.Contains("[Release]", content);
#endif
        Assert.Contains(exception.ToString(), content);
        Assert.Equal(1, handler.Attempts);
    }

    [Fact]
    public async Task Large_exception_is_attached_without_changes_or_truncation()
    {
        var handler = new RecordingHandler();
        using var reporter = CreateReporter(handler);
        var exception = new InvalidOperationException("Authorization: test-token\n" + new string('x', 30000));
        reporter.Notify(exception);
        await handler.Completed.Task.WaitAsync(TimeSpan.FromSeconds(3));
        Assert.NotNull(handler.Attachment);
        Assert.Equal(exception.ToString(), Encoding.UTF8.GetString(handler.Attachment));
    }

    [Fact]
    public void Discord_failure_does_not_escape_to_caller_or_retry()
    {
        var handler = new RecordingHandler { Fail = true };
        using var reporter = CreateReporter(handler);
        reporter.Notify(new Exception("original error"));
        Assert.Equal(1, handler.Attempts);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("https://example.invalid/webhook")]
    public async Task Handler_returns_generic_error_with_or_without_discord_configuration(string? url)
    {
        var transport = new RecordingHandler();
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        using var reporter = CreateReporter(transport, url, context);
        var handler = new ApiExceptionHandler(reporter);
        using var services = new ServiceCollection()
            .AddScoped<INotificationService, NotificationService>()
            .BuildServiceProvider();
        using var scope = services.CreateScope();
        context.RequestServices = scope.ServiceProvider;

        var handled = await handler.TryHandleAsync(context, new InvalidOperationException("private exception detail"), CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(500, context.Response.StatusCode);
        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Contains("An unexpected error occurred", body);
        Assert.DoesNotContain("private exception detail", body);
        Assert.DoesNotContain("InvalidOperationException", body);
        if (string.IsNullOrWhiteSpace(url)) Assert.Equal(0, transport.Attempts);
        else await transport.Completed.Task.WaitAsync(TimeSpan.FromSeconds(3));
    }

    private static DiscordErrorReporter CreateReporter(RecordingHandler handler,
        string? url = "https://example.invalid/webhook", HttpContext? context = null)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["DiscordWebHookURL"] = url
        }).Build();
        return new DiscordErrorReporter(new HttpClient(handler), configuration,
            new HttpContextAccessor { HttpContext = context }, new TestEnvironment());
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public int Attempts;
        public bool Fail;
        public string Payload = "";
        public byte[]? Attachment;
        public TaskCompletionSource Completed = new(TaskCreationOptions.RunContinuationsAsynchronously);
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Attempts++;
            if (Fail) throw new HttpRequestException("simulated Discord failure");
            if (request.Content is MultipartContent multipart)
            {
                foreach (var part in multipart)
                {
                    if (part.Headers.ContentDisposition?.Name?.Trim('"') == "payload_json") Payload = await part.ReadAsStringAsync(cancellationToken);
                    else Attachment = await part.ReadAsByteArrayAsync(cancellationToken);
                }
            }
            else Payload = await request.Content!.ReadAsStringAsync(cancellationToken);
            Completed.TrySetResult();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }

    private sealed class TestEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "Tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = "";
        public string EnvironmentName { get; set; } = "Test";
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
