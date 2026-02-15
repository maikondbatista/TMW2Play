import { Component, Input } from '@angular/core';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { AdvancedPieChartModel } from '../../../shared/models/charts/advanced-pie-chart.model';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';

@Component({
  selector: 'app-last-two-weeks',
  imports: [BaseChartDirective, TranslocoModule],
  standalone: true,
  templateUrl: './last-two-weeks.component.html',
  styleUrl: './last-two-weeks.component.scss'
})
export class LastTwoWeeksComponent {
  @Input() data!: AdvancedPieChartModel[];
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: [],
    datasets: [{ data: [] }]
  };
  public pieChartType: ChartType = 'pie';
  public pieChartOptions: ChartOptions = {
    plugins: {
      legend: {
        display: false,
      }
    },
    responsive: false
  };

  constructor(private translationService: TranslocoService) {
  }
  ngOnInit() {
    const playTimeHours = this.translationService.translate('profile.chartsTab.lastTwoWeeks.playTimeHours');
    setTimeout(() => {
      if (this.data.length > 0) {
        this.pieChartData = {
          labels: this.data.map(d => Array.isArray(d.name) ? d.name : typeof d.name === 'string' && d.name.includes(',') ? d.name.split(',').map(s => s.trim()) : d.name),
          datasets: [
            { data: this.data.map(d => d.value), label: playTimeHours }
          ]
        };
      }
    }, 1000);

  }

  get hasDataWithValue(): boolean {
    return this.data && this.data.length > 0 && this.data.some(item => item.value > 0);
  }
}
