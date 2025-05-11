using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMW2Play.Domain.Core.Gemini
{
    public class GeminiApiConfiguration(string geminiAPIKey)
    {
        private string ApiKey { get; } = geminiAPIKey;
        private string GeminiBaseUrl { get; } = "https://generativelanguage.googleapis.com";
        private string V1Beta { get; } = "/v1beta";
        private string Models { get; } = "/models";
        private string gemini2Flash { get; } = "/gemini-2.0-flash:generateContent";

        public string GeminiPromptUrl(string steamId)
        {
            return $"{GeminiBaseUrl + V1Beta + Models + gemini2Flash}?key={ApiKey}";
        }

    }
}
