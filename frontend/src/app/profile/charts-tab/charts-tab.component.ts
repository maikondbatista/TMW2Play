import { Component, effect, Input, signal, WritableSignal } from '@angular/core';
import { NgbAlertModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { GameModel } from '../../shared/models/steam/owned-games.motel';
import { AdvancedPieChartModel } from '../../shared/models/charts/advanced-pie-chart.model';
import { MinutesToHoursPipe } from '../../shared/pipes/minutes-to-hours.pipe';
import { LastTwoWeeksComponent } from "./last-two-weeks/last-two-weeks.component";
import { AllTimeComponent } from './all-time/all-time.component';
@Component({
  selector: 'app-charts-tab',
  imports: [NgbNavModule, NgbAlertModule, LastTwoWeeksComponent, AllTimeComponent],
  standalone: true,
  providers: [MinutesToHoursPipe],
  templateUrl: './charts-tab.component.html',
  styleUrl: './charts-tab.component.scss'
})
export class ChartsTabComponent {
  @Input() ownedGamesSignal!: WritableSignal<GameModel[]>; // Accept the signal as an input
  view = [700, 400];
  active: number = 1;
  twoWeeksData: AdvancedPieChartModel[] = [];
  allTimeData: AdvancedPieChartModel[] = [];
  gradient: boolean = true;
  showLegend: boolean = true;
  showLabels: boolean = true;
  isDoughnut: boolean = false;

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  constructor(private minutesToHoursPipe: MinutesToHoursPipe) {
    this.signToChartEffects();
  }
  signToChartEffects() {
    this.signTwoWeeksDataEffect();
    this.signAllTimeDataEffect();
  }
  signAllTimeDataEffect() {
    effect(() => {
      this.allTimeData = this.ownedGamesSignal()
        .filter((game) => game.playtime_forever != null && game.playtime_forever > 0)
        .map((game) => {
          return {
            name: game.name,
            value: this.minutesToHoursPipe.transform(game.playtime_forever),
          } as AdvancedPieChartModel;
        });
      console.log(this.twoWeeksData);
    });
  }
  signTwoWeeksDataEffect() {
    effect(() => {
      this.twoWeeksData = this.ownedGamesSignal()
        .filter((game) => game.playtime_2weeks != null && game.playtime_2weeks > 0)
        .map((game) => {
          return {
            name: game.name,
            value: this.minutesToHoursPipe.transform(game.playtime_2weeks),
          } as AdvancedPieChartModel;
        });
      console.log(this.twoWeeksData);
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
