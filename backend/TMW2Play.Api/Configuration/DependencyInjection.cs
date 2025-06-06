using TMW2Play.Service.Services.Notification;

namespace TMW2Play.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<INotificationService, NotificationService>();
            return services;
        }
    }
}
