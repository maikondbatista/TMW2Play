import { AfterViewInit, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, switchMap, take } from 'rxjs';
import { SharedService } from '../shared/services/shared.service';
import { SignalService } from '../shared/services/signal.service';
import { ChartsTabComponent } from './charts-tab/charts-tab.component';
import { OwnedGamesComponent } from './owned-games/owned-games.component';
import { SummaryHeaderComponent } from './summary-header/summary-header.component';
import { TranslocoModule } from '@jsverse/transloco';
import { PlayerModel } from '../shared/models/steam/player-summary.model';
import { GameModel } from '../shared/models/steam/owned-games.model';

const imports = [SummaryHeaderComponent, OwnedGamesComponent, ChartsTabComponent, TranslocoModule]
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
  loading: boolean = true;
  constructor(private ac: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    public signalService: SignalService,
  ) { }

  ngAfterViewInit(): void {
    this.steamUser = this.ac.snapshot.paramMap.get('steamUser');
    const type = this.ac.snapshot.queryParamMap.get('type');

    if (!this.steamUser) {
      this.router.navigate(['Home']);
      return;
    }

    type === 'id'
      ? this.loadProfileById(this.steamUser)
      : this.loadProfile(this.steamUser);
  }

  private loadProfileById(steamId: string): void {
    this.loading = true;
    this.loadProfileData(steamId);
  }

  private loadProfile(steamUser: string): void {
    this.loading = true;
    this.sharedService.getUserId(steamUser)
      .pipe(
        take(1)
      ).subscribe(id => {
        this.loadProfileData(id)
      });
  }

  private loadProfileData(steamId: string) {
    return forkJoin([
      this.sharedService.playerSummary(steamId),
      this.sharedService.steamOwnedGames(steamId)
    ]).subscribe(([playerSummary, ownedGames]) => {
      this.updateSignals(playerSummary, ownedGames);
      this.loading = false;
    });
  }

  private updateSignals(playerSummary: PlayerModel, ownedGames: GameModel[]): void {
    this.signalService.allGamesSignal.set(ownedGames);
    this.signalService.playerSummarySignal.set(playerSummary);
    this.signalService.ownedGamesSignal.set(ownedGames);
  }

  public onHideNeverPlayed(hideNeverPlayed: boolean): void {
    const games = hideNeverPlayed
      ? this.signalService.allGamesSignal().filter(game => game.playtime_forever > 0)
      : this.signalService.allGamesSignal();

    this.signalService.ownedGamesSignal.set(games);
  }
}