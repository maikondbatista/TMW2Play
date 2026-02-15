import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { TranslocoModule } from '@jsverse/transloco';
import { SharedService } from '../../../shared/services/shared.service';
import { SignalService } from '../../../shared/services/signal.service';
import { UpcomingGameModel } from '../../../shared/models/llm/upcoming-game.model';
import { LLMBaseComponent } from '../shared/llm-base.component';

@Component({
  selector: 'app-tell-me-what-is-upcoming',
  imports: [TranslocoModule],
  templateUrl: './tell-me-what-is-upcoming.component.html',
  styleUrl: './tell-me-what-is-upcoming.component.scss',
})
export class TellMeWhatIsUpcomingComponent extends LLMBaseComponent<UpcomingGameModel> {
  translationPrefix = 'profile.chartsTab.tellMeWhatIsUpcoming.';
  locale!: string;
  constructor(
    sharedService: SharedService,
    signalService: SignalService
  ) {
    super(sharedService, signalService);
  }

  protected callService(
    lastTwoWeeks: string[],
    allGames: string[]
  ): Observable<UpcomingGameModel[]> {
    return this.sharedService.tellMeWhatIsUpcoming(lastTwoWeeks, allGames);
  }

  get upcomingGames(): UpcomingGameModel[] {
    return this.data;
  }
}

