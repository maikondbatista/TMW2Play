import { Component, Input } from '@angular/core';
import { AdvancedPieChartModel } from '../../../shared/models/charts/advanced-pie-chart.model';
import { NgxChartsModule } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-all-time',
  imports: [NgxChartsModule],
  standalone: true,
  templateUrl: './all-time.component.html',
  styleUrl: './all-time.component.scss'
})
export class AllTimeComponent {
  @Input() data!: AdvancedPieChartModel[];
}
