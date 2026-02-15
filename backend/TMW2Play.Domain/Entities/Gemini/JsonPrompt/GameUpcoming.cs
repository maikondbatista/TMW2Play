namespace TMW2Play.Domain.Entities.Gemini.JsonPrompt
{
    public class GameUpcoming
    {
        public int id { get; set; }
        public string name { get; set; }
        public string releaseDate { get; set; }
        public List<string> genres { get; set; }
        public List<string> platforms { get; set; }
        public string description { get; set; }
        public int anticipation { get; set; }
    }
}
