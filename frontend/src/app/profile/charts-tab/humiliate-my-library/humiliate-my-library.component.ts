import { Component, effect } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { take } from 'rxjs';
import { TwoWeeksFilter } from '../../../shared/constants/filters/two-weeks.filter';
import { SharedService } from '../../../shared/services/shared.service';
import { SignalService } from '../../../shared/services/signal.service';
import { TranslocoModule } from '@jsverse/transloco';
import { MinutesToHoursPipe } from '../../../shared/pipes/minutes-to-hours.pipe';

@Component({
  selector: 'app-humiliate-my-library',
  imports: [TranslocoModule, MinutesToHoursPipe],
  templateUrl: './humiliate-my-library.component.html',
  styleUrl: './humiliate-my-library.component.scss'
})
export class HumiliateMyLibraryComponent {

  humiliationText: string = "Humuliation in progress...";
  humiliationHtml: SafeHtml = '';
  loading: boolean = false;
  translationPrefix = 'profile.chartsTab.humiliateMyLibrary.';

  constructor(private sharedService: SharedService, private signalService: SignalService, private sanitizer: DomSanitizer, private minutesToHoursPipe: MinutesToHoursPipe) {
    this.signToEffects();
  }

  private signToEffects() {
    effect(() => {
      let lastTwoWeeks = this.signalService.allGamesSignal().filter(TwoWeeksFilter.filter);
      let allGames = this.signalService.allGamesSignal();
      if (lastTwoWeeks.length > 0 || allGames.length > 0) {
        this.loading = true;
        this.sharedService.humiliateMyLibrary({
          lastTwoWeeks: lastTwoWeeks.map(g => ({ game: g.name, time: this.minutesToHoursPipe.transform(g.playtime_2weeks) })),
          allGames: allGames.map(g => ({ game: g.name, time: this.minutesToHoursPipe.transform(g.playtime_forever) }))
        }).pipe(take(1))
          .subscribe((response) => {
            this.humiliationText = response;
            this.humiliationHtml = this.sanitizer.bypassSecurityTrustHtml(this.markdownToHtml(response));
            this.loading = false;
          });
      }
    });
  }

  private markdownToHtml(markdown: string): string {
    return markdown
      .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')     // **bold**
      .replace(/\*(.+?)\*/g, '<strong>$1</strong>')         // *italic*
      .replace(/\n\n/g, '</p><p>')                          // paragraphs
      .replace(/\n/g, '<br>')                               // line breaks
      .replace(/^/, '<p>')
      .replace(/$/, '</p>');
  }
}
