import { LowerCasePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PlayerModel } from '../../shared/models/steam/player-summary.model';
import { TranslocoModule } from '@jsverse/transloco';

@Component({
  selector: 'app-summary-header',
  imports: [LowerCasePipe, FormsModule, TranslocoModule],
  templateUrl: './summary-header.component.html',
  styleUrl: './summary-header.component.scss'
})
export class SummaryHeaderComponent {
  switchNeverPlayed: boolean = false;
  translationPrefix = 'profile.summaryHeader.';
  @Input() playerSummary!: PlayerModel;
  @Output() hideNeverPlayed: EventEmitter<any> = new EventEmitter<any>();
}
