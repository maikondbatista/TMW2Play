namespace TMW2Play.Domain.DTO
{
    public class TellMeWhatToPlayRequest
    {
        public List<string> LastTwoWeeks { get; set; }
        public List<string> AllGames { get; set; }
    }
}
