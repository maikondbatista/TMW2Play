import { AfterViewInit, Component, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, switchMap, take } from 'rxjs';
import { SharedService } from '../shared/services/shared.service';
import { GameModel } from '../shared/models/steam/owned-games.motel';
import { PlayerModel } from '../shared/models/steam/player-summary.model';
import { SummaryHeaderComponent } from './summary-header/summary-header.component';
import { OwnedGamesComponent } from './owned-games/owned-games.component';
import { LastPlayedPipe } from '../shared/pipes/last-played.pipe';
import { PlayTimePipe } from '../shared/pipes/play-time.pipe';
import { ChartsTabComponent } from './charts-tab/charts-tab.component';

const imports = [SummaryHeaderComponent, OwnedGamesComponent, ChartsTabComponent]
@Component({
  selector: 'app-profile',
  imports: [imports],
  providers: [SharedService],
  standalone: true,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements AfterViewInit {
  steamUser: string | null = null;
  ownedGamesSignal: WritableSignal<GameModel[]> = signal([]);
  playerSummary!: PlayerModel;
  allGamesSignal: WritableSignal<GameModel[]> = signal([]);

  constructor(private ac: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService) { }

  ngAfterViewInit(): void {
    this.steamUser = this.ac.snapshot.paramMap.get('steamUser');
    if (this.steamUser) {
      this.loadProfile(this.steamUser);
    } else {
      this.router.navigate(['Home']);
    }
  }

  public onHideNeverPlayed(hideNeverPlayed: boolean): void {
    if (hideNeverPlayed) {
      return this.ownedGamesSignal.set(this.allGamesSignal().filter(game => game.playtime_forever > 0));
    }
    this.ownedGamesSignal.set(this.allGamesSignal().filter(game => game.playtime_forever === 0));
  }

  private async loadProfile(steamUser: string): Promise<void> {
    this.sharedService.getUserId(steamUser)
      .pipe(
        take(1),
        switchMap((steamId) => {
          return forkJoin([
            this.sharedService.playerSummary(steamId),
            this.sharedService.steamOwnedGames(steamId)
          ]);
        })).subscribe(([playerSummary, ownedGames]) => {
          setTimeout(() => {
            this.allGamesSignal.set(ownedGames);
            this.playerSummary = playerSummary;
            this.ownedGamesSignal.set(ownedGames); // Update the signal
          }, 1000);
        });
  }

}
