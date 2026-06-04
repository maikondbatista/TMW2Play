using TMW2Play.Domain.Core.OpenRouter;
using TMW2Play.Domain.Interfaces.Services;
using TMW2Play.Service.Services.OpenRouter;

namespace TMW2Play.Api.Configuration
{
    public static class AIConfiguration
    {
        public static IServiceCollection AddOpenRouterAi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOpenRouterHttpService, OpenRouterHttpService>();
            services.AddOpenRouterAiKey(configuration);
            return services;
        }

        private static IServiceCollection AddOpenRouterAiKey(this IServiceCollection services, IConfiguration configuration)
        {
            var openRouterAPIKey = configuration["OpenRouterAPIKey"];
            var model = configuration["OpenRouterModel"];
            services.AddSingleton(new OpenRouterApiConfiguration(openRouterAPIKey, model));
            return services;
        }
    }
}
