import { Component, effect } from '@angular/core';
import { NgbAlertModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { AdvancedPieChartModel } from '../../shared/models/charts/advanced-pie-chart.model';
import { MinutesToHoursPipe } from '../../shared/pipes/minutes-to-hours.pipe';
import { LastTwoWeeksComponent } from "./last-two-weeks/last-two-weeks.component";
import { AllTimeComponent } from './all-time/all-time.component';
import { TellMeWhatToPlayComponent } from './tell-me-what-to-play/tell-me-what-to-play.component';
import { HumiliateMyLibraryComponent } from "./humiliate-my-library/humiliate-my-library.component";
import { TwoWeeksFilter } from '../../shared/constants/filters/two-weeks.filter';
import { SignalService } from '../../shared/services/signal.service';
const imports = [NgbNavModule, NgbAlertModule, LastTwoWeeksComponent, AllTimeComponent,
  TellMeWhatToPlayComponent, HumiliateMyLibraryComponent, HumiliateMyLibraryComponent];
@Component({
  selector: 'app-charts-tab',
  imports: [imports],
  standalone: true,
  providers: [MinutesToHoursPipe],
  templateUrl: './charts-tab.component.html',
  styleUrl: './charts-tab.component.scss'
})
export class ChartsTabComponent {
  view = [700, 400];
  active: number = 1;
  twoWeeksData: AdvancedPieChartModel[] = [];
  allTimeData: AdvancedPieChartModel[] = [];
  gradient: boolean = true;
  showLegend: boolean = true;
  showLabels: boolean = true;
  isDoughnut: boolean = false;
  tmw2PlayLoaded: boolean = false;
  
  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  constructor(private minutesToHoursPipe: MinutesToHoursPipe, private signalService: SignalService) {
    this.signToChartEffects();
  }
  signToChartEffects() {
    this.signTwoWeeksDataEffect();
    this.signAllTimeDataEffect();
  }

  signAllTimeDataEffect() {
    effect(() => {
      this.allTimeData = this.signalService.allGamesSignal()
        .filter((game) => game.playtime_forever != null && game.playtime_forever > 0)
        .map((game) => {
          return {
            name: game.name,
            value: this.minutesToHoursPipe.transform(game.playtime_forever),
          } as AdvancedPieChartModel;
        });
    });
  }
  signTwoWeeksDataEffect() {
    effect(() => {
      this.twoWeeksData = this.signalService.allGamesSignal()
        .filter(TwoWeeksFilter.filter)
        .map((game) => {
          return {
        name: game.name,
        value: this.minutesToHoursPipe.transform(game.playtime_2weeks),
          } as AdvancedPieChartModel;
        });
    });
  }


  onSelect(data: any): void {
    console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }

  onActivate(data: any): void {
    console.log('Activate', JSON.parse(JSON.stringify(data)));
  }

  onDeactivate(data: any): void {
    console.log('Deactivate', JSON.parse(JSON.stringify(data)));
  }
}
