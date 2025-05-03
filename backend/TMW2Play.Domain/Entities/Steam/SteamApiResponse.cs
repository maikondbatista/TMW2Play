namespace TMW2Play.Domain.Entities.Steam
{
    public class SteamApiResponse<T> where T : SteamEntity
    {
        public T Response { get; set; }
    }
}
