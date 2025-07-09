import { Pipe, PipeTransform } from '@angular/core';
import { TranslocoService } from '@jsverse/transloco';

@Pipe({
  name: 'playTime'
})
export class PlayTimePipe implements PipeTransform {

  constructor(private translationService: TranslocoService) {
  }
  transform(unixTime: number): string {
    let hoursLabel = this.translationService.translate('common.hour') + 's';
    let minutesLabel = this.translationService.translate('common.minute') + 's';

    const hours = Math.floor(unixTime / 60);
    const minutes = unixTime % 60;
    return `${hours} ${hoursLabel} ${minutes} ${minutesLabel}`;
  }

}
