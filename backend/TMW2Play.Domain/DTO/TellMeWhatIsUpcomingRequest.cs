namespace TMW2Play.Domain.DTO
{
    public class TellMeWhatIsUpcomingRequest
    {
        public IEnumerable<string> LastTwoWeeks { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> AllGames { get; set; } = Enumerable.Empty<string>();
    }

}
