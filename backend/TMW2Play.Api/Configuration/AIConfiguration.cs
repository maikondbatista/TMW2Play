using TMW2Play.Domain.Core.Gemini;
using TMW2Play.Infra.HTTP.Gemini;

namespace TMW2Play.Api.Configuration
{
    public static class AIConfiguration
    {
        public static IServiceCollection AddGeminiAi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGeminiHttpService, GeminiHttpService>();
            services.AddGeminiAiKey(configuration);
            return services;
        }
        private static IServiceCollection AddGeminiAiKey(this IServiceCollection services, IConfiguration configuration)
        {
            var geminiAPIKey = configuration["GeminiAPIKey"];
            services.AddSingleton(new GeminiApiConfiguration(geminiAPIKey));
            return services;
        }
    }
}
