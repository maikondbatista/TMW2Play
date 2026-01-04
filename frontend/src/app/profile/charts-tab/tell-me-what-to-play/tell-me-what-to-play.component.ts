import { Component, effect } from '@angular/core';
import { SharedService } from '../../../shared/services/shared.service';
import { take } from 'rxjs';
import { TwoWeeksFilter } from '../../../shared/constants/filters/two-weeks.filter';
import { SignalService } from '../../../shared/services/signal.service';
import { RecommendationModel } from '../../../shared/models/llm/recommendation.model';

import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-tell-me-what-to-play',
  imports: [TranslocoModule],
  templateUrl: './tell-me-what-to-play.component.html',
  styleUrl: './tell-me-what-to-play.component.scss'
})
export class TellMeWhatToPlayComponent {
  TMW2Play: RecommendationModel[] = []
  loading: boolean = false;
  translationPrefix = 'profile.chartsTab.tellMeWhatToPlay.';
  
  constructor(private sharedService: SharedService, private signalService: SignalService) {
    this.signToEffects();
  }

  private signToEffects() {
    effect(() => {
      let lastTwoWeeks = this.signalService.allGamesSignal().filter(TwoWeeksFilter.filter).map(s => s.name);
      let allGames = this.signalService.allGamesSignal().map(s => s.name);
      if (lastTwoWeeks.length > 0 || allGames.length > 0) {
          this.loading = true;
          this.sharedService.tellMeWhatToPlay(lastTwoWeeks, allGames).pipe(take(1)).subscribe((response) => {
          this.TMW2Play = response;
          this.loading = false;
       });
      }
    });
  }
}
