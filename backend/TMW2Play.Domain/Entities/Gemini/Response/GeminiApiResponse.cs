namespace TMW2Play.Domain.Entities.Gemini.Response
{
    public class Candidate
    {
        public Content Content { get; set; }
    }
    public class Content
    {
        public List<PartResponse> Parts { get; set; }
    }
    public class PartResponse
    {
        public string Text { get; set; }
    }

    public class GeminiApiResponse
    {
        public List<Candidate> Candidates { get; set; }
    }
}
