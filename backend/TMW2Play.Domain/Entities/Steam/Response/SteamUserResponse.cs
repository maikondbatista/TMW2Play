namespace TMW2Play.Domain.Entities.Steam
{
    public class SteamUserResponse : SteamEntity
    {
        public string steamid { get; set; }
        public int success { get; set; }
    }
}
