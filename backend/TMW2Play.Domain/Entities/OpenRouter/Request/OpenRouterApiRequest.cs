using System.Text.Json.Serialization;

namespace TMW2Play.Domain.Entities.OpenRouter.Request
{
    public class OpenRouterApiRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
        public Reasoning Reasoning { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
    
    public class Reasoning
    {
        public bool Enabled { get; set; } = false;
    }
}
