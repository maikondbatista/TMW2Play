import { Component, Input } from '@angular/core';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { AdvancedPieChartModel } from '../../../shared/models/charts/advanced-pie-chart.model';

@Component({
  selector: 'app-all-time',
  imports: [BaseChartDirective],
  standalone: true,
  templateUrl: './all-time.component.html',
  styleUrl: './all-time.component.scss'
})
export class AllTimeComponent {
  @Input() data!: AdvancedPieChartModel[];

  public pieChartData: ChartConfiguration<'pie'>['data'] = {
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
responsive: false  };
  ngOnInit() {
    setTimeout(() => {
      if (this.data.length > 0) {
        this.pieChartData = {
          labels: this.data.map(d => Array.isArray(d.name) ? d.name : typeof d.name === 'string' && d.name.includes(',') ? d.name.split(',').map(s => s.trim()) : d.name),
          datasets: [
            { data: this.data.map(d => d.value), label: 'Playtime(hours)' }
          ]
        };
      }
    }, 1000);
  }
}
