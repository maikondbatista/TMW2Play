namespace TMW2Play.Domain.DTO
{
    public class    HumiliateMyLibraryRequest
    {
        public IEnumerable<HumiliateMyLibraryGameRequest> LastTwoWeeks { get; set; } = Enumerable.Empty<HumiliateMyLibraryGameRequest>();
        public IEnumerable<HumiliateMyLibraryGameRequest> AllGames { get; set; } = Enumerable.Empty<HumiliateMyLibraryGameRequest>();
    }

    public class HumiliateMyLibraryGameRequest
    {
        public string Game { get; set; }
        public decimal? Time { get; set; }
    }
}
