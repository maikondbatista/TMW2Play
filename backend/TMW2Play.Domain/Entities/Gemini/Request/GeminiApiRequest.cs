namespace TMW2Play.Domain.Entities.Gemini.Request
{
    public class GeminiApiRequest
    {
        public List<Content> Contents { get; set; }
    }

    public class Content
    {
        public List<Part> Parts { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }
}
