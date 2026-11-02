using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;
using TMW2Play.Service.Services.Http;
using TMW2Play.Service.Services.Notification;
using TMW2Play.Api.ErrorReporting;

namespace TMW2Play.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiscordNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddExceptionHandler<ApiExceptionHandler>();
            services.AddProblemDetails();

            services.AddSingleton(sp => new DiscordErrorReporter(
                new HttpClient { Timeout = TimeSpan.FromSeconds(5) }, configuration,
                sp.GetRequiredService<IHttpContextAccessor>(), sp.GetRequiredService<IWebHostEnvironment>()));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHttpService, HttpService>();
            return services;
        }
    }
}
