namespace TMW2Play.Domain.Entities.Steam
{
    public class OwnedGamesResponse : SteamEntity
    {
        public int game_count { get; set; }
        public List<OwnedGameResponse> games { get; set; }
    }
    public class OwnedGameResponse
    {
        public int appid { get; set; }
        public string name { get; set; }
        public int playtime_2weeks { get; set; }
        public int playtime_forever { get; set; }
        public string img_icon_url { get; set; }
        public int playtime_windows_forever { get; set; }
        public int playtime_mac_forever { get; set; }
        public int playtime_linux_forever { get; set; }
        public int playtime_deck_forever { get; set; }
        public int rtime_last_played { get; set; }
        public List<int> content_descriptorids { get; set; }
        public int playtime_disconnected { get; set; }
    }
}
