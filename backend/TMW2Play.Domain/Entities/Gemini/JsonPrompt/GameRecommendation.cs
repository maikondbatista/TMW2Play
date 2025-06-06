namespace TMW2Play.Domain.Entities.Gemini.JsonPrompt
{
    public class GameRecommendation
    {
        public int id { get; set; }
        public List<string> genres { get; set; }
        public string referenceGame { get; set; }
        public string name { get; set; }
        public string pitch { get; set; }
        public string why { get; set; }
        public string score { get; set; }
        public bool isWildcard { get; set; } = false;
    }
}
