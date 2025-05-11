using TMW2Play.Domain.Core.Gemini;
using TMW2Play.Infra.HTTP.Steam;

namespace TMW2Play.Api.Configuration
{
    public static class AIConfiguration
    {
        public static IServiceCollection AddGeminiAi(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IGeminiService, GeminiService>();
            return services;
        }
        public static IServiceCollection AddGeminiAiKey(this IServiceCollection services, IConfiguration configuration)
        {
            var geminiAPIKey = configuration["GeminiAPIKey"];
            services.AddSingleton(new GeminiApiConfiguration(geminiAPIKey));
            return services;
        }
    }
}
