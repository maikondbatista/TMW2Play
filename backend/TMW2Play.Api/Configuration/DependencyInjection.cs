using TMW2Play.Infra.HTTP.Steam;

namespace TMW2Play.Api.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISteamHttpService, SteamHttpService>();
            return services;
        }
    }
}
