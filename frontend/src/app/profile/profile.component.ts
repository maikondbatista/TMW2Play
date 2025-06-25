import { AfterViewInit, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, switchMap, take } from 'rxjs';
import { SharedService } from '../shared/services/shared.service';
import { SignalService } from '../shared/services/signal.service';
import { ChartsTabComponent } from './charts-tab/charts-tab.component';
import { OwnedGamesComponent } from './owned-games/owned-games.component';
import { SummaryHeaderComponent } from './summary-header/summary-header.component';

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
  loading: boolean = true;
  constructor(private ac: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService,
    public signalService: SignalService,
  ) { }

  ngAfterViewInit(): void {
    this.steamUser = this.ac.snapshot.paramMap.get('steamUser');
    if (this.steamUser) {
      this.loadProfile(this.steamUser);
    } else {
      // this.router.navigate(['Home']);
    }
  }

  public onHideNeverPlayed(hideNeverPlayed: boolean): void {
    if (hideNeverPlayed) {
      return this.signalService.ownedGamesSignal.set(this.signalService.allGamesSignal().filter(game => game.playtime_forever > 0));
    }
    this.signalService.ownedGamesSignal.set(this.signalService.allGamesSignal());
  }

  private async loadProfile(steamUser: string): Promise<void> {
    this.loading = true;
    this.sharedService.getUserId(steamUser)
      .pipe(
        take(1),
        switchMap((steamId) => {
          return forkJoin([
            this.sharedService.playerSummary(steamId),
            this.sharedService.steamOwnedGames(steamId)
          ]);
        })).subscribe(([playerSummary, ownedGames]) => {
          this.signalService.allGamesSignal.set(ownedGames);
          this.signalService.playerSummarySignal.set(playerSummary);
          this.signalService.ownedGamesSignal.set(ownedGames);
          this.loading = false;
        });
  }

}
