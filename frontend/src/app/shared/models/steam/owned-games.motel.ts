export interface GameModel {
    appid: number;
    name: string;
    playtime_2weeks: number;
    playtime_forever: number;
    img_icon_url: string;
    playtime_windows_forever: number;
    playtime_mac_forever: number;
    playtime_linux_forever: number;
    playtime_deck_forever: number;
    rtime_last_played: number;
    content_descriptorids?: any;
    playtime_disconnected: number;
}

export interface OwnedGamesModel {
    game_count: number;
    games: GameModel[];
}