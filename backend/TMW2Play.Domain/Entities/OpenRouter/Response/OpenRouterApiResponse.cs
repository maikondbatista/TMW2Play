using System.Text.Json.Serialization;

namespace TMW2Play.Domain.Entities.OpenRouter.Response
{
    public class OpenRouterApiResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public MessageResponse Message { get; set; }
    }

    public class MessageResponse
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
