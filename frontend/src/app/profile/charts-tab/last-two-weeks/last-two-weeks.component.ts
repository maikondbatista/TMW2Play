import { Component, Input } from '@angular/core';
import { AdvancedPieChartModel } from '../../../shared/models/charts/advanced-pie-chart.model';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-last-two-weeks',
  imports: [NgxChartsModule],
  standalone: true,
  templateUrl: './last-two-weeks.component.html',
  styleUrl: './last-two-weeks.component.scss'
})
export class LastTwoWeeksComponent {
  @Input() data!: AdvancedPieChartModel[];

  teste(data: any): any{
    return data + 'TESTANDO';
  }
}
