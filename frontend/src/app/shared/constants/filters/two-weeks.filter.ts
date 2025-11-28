import { GameModel } from "../../models/steam/owned-games.model";

export class TwoWeeksFilter {
    public static filter = (game: GameModel) => game.playtime_2weeks != null && game.playtime_2weeks > 0;
}