import { Component, effect, Input, OnInit, WritableSignal } from '@angular/core';
import { GameModel } from '../../shared/models/steam/owned-games.motel';
import { LastPlayedPipe } from '../../shared/pipes/last-played.pipe';
import { PlayTimePipe } from '../../shared/pipes/play-time.pipe';

@Component({
  selector: 'app-owned-games',
  imports: [LastPlayedPipe, PlayTimePipe],
  standalone: true,
  templateUrl: './owned-games.component.html',
  styleUrl: './owned-games.component.scss'
})
export class OwnedGamesComponent {

  @Input() ownedGamesSignal!: WritableSignal<GameModel[]>; // Accept the signal as an input
  ownedGames: GameModel[] = [];
  getGameImageUrl(game: GameModel): string {
    return `https://media.steampowered.com/steamcommunity/public/images/apps/${game.appid}/${game.img_icon_url}.jpg`;
  }
  
  constructor() {
    effect(() => {
      if (this.ownedGamesSignal) {
        this.ownedGames = this.ownedGamesSignal();
      }
    });
  }

}
