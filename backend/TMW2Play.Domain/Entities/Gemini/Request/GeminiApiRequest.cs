namespace TMW2Play.Domain.Entities.Gemini.Request
{
    public class GeminiApiRequest
    {
        public List<Content> Contents { get; set; }
    }

    public class Content
    {
        public List<PartRequest> Parts { get; set; }
    }

    public class PartRequest
    {
        public string Text { get; set; }
    }
}
