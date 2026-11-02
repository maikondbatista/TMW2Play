using System.Net;
using System.Text.Json;
using TMW2Play.Domain.Core.OpenRouter;
using TMW2Play.Domain.Entities.OpenRouter.Response;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Services.Http;
using TMW2Play.Service.Services.Notification;
using TMW2Play.Service.Services.OpenRouter;

namespace TMW2Play.Api.Tests;

public class DependencyExceptionTests
{
    [Fact]
    public async Task Http_service_propagates_original_network_exception()
    {
        var original = new HttpRequestException("original network failure");
        using var client = new HttpClient(new TestHandler(_ => throw original));
        var service = new HttpService(client);
        var actual = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAsync<object>("https://example.invalid", CancellationToken.None));
        Assert.Same(original, actual);
    }

    [Fact]
    public async Task Http_error_status_becomes_exception_instead_of_notification()
    {
        using var client = new HttpClient(new TestHandler(_ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)));
        var service = new HttpService(client);
        var actual = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAsync<object>("https://example.invalid", CancellationToken.None));
        Assert.Equal(HttpStatusCode.ServiceUnavailable, actual.StatusCode);
    }

    [Fact]
    public async Task OpenRouter_keeps_business_validation_as_notification()
    {
        var notifications = new NotificationService();
        var service = new OpenRouterHttpService(new InvalidJsonHttpService(), new OpenRouterApiConfiguration("test-key"), notifications);
        var result = await service.TellMeWhatToPlay([], [], "en-US");
        Assert.Empty(result);
        Assert.Equal("No games added to library.", Assert.Single(notifications.Notifications()));
    }

    [Fact]
    public async Task OpenRouter_json_exception_is_not_converted_to_user_notification()
    {
        var notifications = new NotificationService();
        var service = new OpenRouterHttpService(new InvalidJsonHttpService(), new OpenRouterApiConfiguration("test-key"), notifications);
        await Assert.ThrowsAnyAsync<JsonException>(() => service.TellMeWhatToPlay(["Game"], ["Game"], "en-US"));
        Assert.Empty(notifications.Notifications());
    }

    private sealed class TestHandler(Func<HttpRequestMessage, HttpResponseMessage> response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(response(request));
    }

    private sealed class InvalidJsonHttpService : IHttpService
    {
        public Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken, Dictionary<string, string>? headers = null) => throw new NotSupportedException();
        public Task<T?> PostAsync<T>(string url, object body, CancellationToken cancellationToken, Dictionary<string, string>? headers = null)
        {
            object response = new OpenRouterApiResponse { Choices = [new Choice { Message = new MessageResponse { Content = "[invalid json" } }] };
            return Task.FromResult((T?)response);
        }
    }
}
