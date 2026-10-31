using System.Text.Json.Serialization;

namespace TMW2Play.Domain.Entities.OpenRouter.Request
{
    public class OpenRouterApiRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("plugins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<OpenRouterPlugin>? Plugins { get; set; }

        [JsonPropertyName("response_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OpenRouterResponseFormat? ResponseFormat { get; set; }

        public Reasoning Reasoning { get; set; }
    }

    public class OpenRouterPlugin
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class OpenRouterResponseFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
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
