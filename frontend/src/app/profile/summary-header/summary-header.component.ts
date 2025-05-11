import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlayerModel } from '../../shared/models/steam/player-summary.model';
import { LowerCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-summary-header',
  imports: [LowerCasePipe, FormsModule],
  templateUrl: './summary-header.component.html',
  styleUrl: './summary-header.component.scss'
})
export class SummaryHeaderComponent {
  switchNeverPlayed: boolean = false;
  @Input() playerSummary!: PlayerModel;
  @Output() hideNeverPlayed: EventEmitter<any> = new EventEmitter<any>();
}
