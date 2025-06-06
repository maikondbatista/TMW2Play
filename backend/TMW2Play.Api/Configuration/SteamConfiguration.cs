using Microsoft.Extensions.Configuration;
using TMW2Play.Domain.Core.Steam;
using TMW2Play.Infra.HTTP.Steam;

namespace TMW2Play.Api.Configuration
{
    public static class SteamConfiguration
    {
        public static IServiceCollection AddSteam(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISteamHttpService, SteamHttpService>();
            services.AddSteamKey(configuration);
            return services;
        }
        private static IServiceCollection AddSteamKey(this IServiceCollection services, IConfiguration configuration)
        {
            var steamApiKey = configuration["SteamAPIKey"];
            services.AddSingleton(new SteamApiConfiguration(steamApiKey));
            return services;
        }
    }
}
