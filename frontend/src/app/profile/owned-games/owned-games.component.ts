import { Component, effect } from '@angular/core';
import { GameModel } from '../../shared/models/steam/owned-games.motel';
import { LastPlayedPipe } from '../../shared/pipes/last-played.pipe';
import { PlayTimePipe } from '../../shared/pipes/play-time.pipe';
import { SignalService } from '../../shared/services/signal.service';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-owned-games',
  imports: [LastPlayedPipe, PlayTimePipe, TranslocoModule],
  standalone: true,
  templateUrl: './owned-games.component.html',
  styleUrl: './owned-games.component.scss'
})
export class OwnedGamesComponent {
  ownedGames: GameModel[] = [];
  translationPrefix = 'profile.ownedGames.';

  getGameImageUrl(game: GameModel): string {
    return `https://media.steampowered.com/steamcommunity/public/images/apps/${game.appid}/${game.img_icon_url}.jpg`;
  }
  
  constructor(signalService: SignalService) {
    effect(() => {
        this.ownedGames = signalService.ownedGamesSignal();
    });
  }

}
