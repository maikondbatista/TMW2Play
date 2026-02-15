import { effect } from '@angular/core';
import { take, finalize } from 'rxjs';
import { Observable } from 'rxjs';
import { TwoWeeksFilter } from '../../../shared/constants/filters/two-weeks.filter';
import { SharedService } from '../../../shared/services/shared.service';
import { SignalService } from '../../../shared/services/signal.service';

export abstract class LLMBaseComponent<T> {
  data: T[] = [];
  loading: boolean = false;
  abstract translationPrefix: string;

  constructor(
    protected sharedService: SharedService,
    protected signalService: SignalService
  ) {
    this.signToEffects();
  }

  protected abstract callService(
    lastTwoWeeks: string[],
    allGames: string[]
  ): Observable<T[]>;

  private signToEffects() {
    effect(() => {
      const games = this.signalService.allGamesSignal();
      const lastTwoWeeks = games.filter(TwoWeeksFilter.filter).map(s => s.name);
      const allGames = games.map(s => s.name);

      if (lastTwoWeeks.length > 0 || allGames.length > 0) {
        this.loading = true;
        this.callService(lastTwoWeeks, allGames)
          .pipe(
            take(1),
            finalize(() => (this.loading = false))
          )
          .subscribe(response => (this.data = response));
      }
    });
  }
}
