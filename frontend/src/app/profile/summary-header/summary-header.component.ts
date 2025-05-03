import { Component, Input } from '@angular/core';
import { PlayerModel } from '../../shared/models/steam/player-summary.model';
import { LowerCasePipe } from '@angular/common';

@Component({
  selector: 'app-summary-header',
  imports: [LowerCasePipe],
  templateUrl: './summary-header.component.html',
  styleUrl: './summary-header.component.scss'
})
export class SummaryHeaderComponent {
@Input() playerSummary!: PlayerModel;
}
