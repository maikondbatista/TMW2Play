using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TMW2Play.Service.Domain.Services;

namespace TMW2Play.Api.ErrorReporting;

public sealed class ApiExceptionHandler(DiscordErrorReporter reporter) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        reporter.Notify(exception);
        var notificationService = context.RequestServices.GetRequiredService<INotificationService>();
        notificationService.AddNotification("An unexpected error occurred. Please try again later.");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(notificationService.Notifications(), cancellationToken);
        return true;
    }
}
