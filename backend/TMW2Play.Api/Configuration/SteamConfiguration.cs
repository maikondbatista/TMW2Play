using TMW2Play.Domain.Core.Steam;
using TMW2Play.Infra.HTTP.Steam;

namespace TMW2Play.Api.Configuration
{
    public static class SteamConfiguration
    {
        public static IServiceCollection AddSteam(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<ISteamHttpService, SteamHttpService>();
            return services;
        }
        public static IServiceCollection AddSteamKey(this IServiceCollection services, IConfiguration configuration)
        {
            var steamApiKey = configuration["SteamAPIKey"];
            services.AddSingleton(new SteamApiConfiguration(steamApiKey));
            return services;
        }
    }
}
