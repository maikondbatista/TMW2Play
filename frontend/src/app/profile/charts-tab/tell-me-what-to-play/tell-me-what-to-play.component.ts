import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { SharedService } from '../../../shared/services/shared.service';
import { SignalService } from '../../../shared/services/signal.service';
import { RecommendationModel } from '../../../shared/models/llm/recommendation.model';
import { TranslocoModule } from '@jsverse/transloco';
import { LLMBaseComponent } from '../shared/llm-base.component';

@Component({
  selector: 'app-tell-me-what-to-play',
  imports: [TranslocoModule],
  templateUrl: './tell-me-what-to-play.component.html',
  styleUrl: './tell-me-what-to-play.component.scss'
})
export class TellMeWhatToPlayComponent extends LLMBaseComponent<RecommendationModel> {
  translationPrefix = 'profile.chartsTab.tellMeWhatToPlay.';

  constructor(
    sharedService: SharedService,
    signalService: SignalService
  ) {
    super(sharedService, signalService);
  }

  protected callService(
    lastTwoWeeks: string[],
    allGames: string[]
  ): Observable<RecommendationModel[]> {
    return this.sharedService.tellMeWhatToPlay(lastTwoWeeks, allGames);
  }

  get TMW2Play(): RecommendationModel[] {
    return this.data;
  }
}
