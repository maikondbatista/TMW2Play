using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Domain.Services;
using TMW2Play.Service.Services.Http;
using TMW2Play.Service.Services.Notification;

namespace TMW2Play.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHttpService, HttpService>();
            return services;
        }
    }
}
