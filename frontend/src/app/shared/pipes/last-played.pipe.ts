import { Pipe, PipeTransform } from '@angular/core';
import { TranslocoService } from '@jsverse/transloco';

@Pipe({
  name: 'lastPlayed'
})
export class LastPlayedPipe implements PipeTransform {

  constructor(private translationService: TranslocoService) {
  }
  transform(unixTime: number): string {
    const agoLabel = this.translationService.translate('common.ago');
    const justNowLabel = this.translationService.translate('common.justNow');
    const neverPlayedLabel = this.translationService.translate('common.neverPlayed');

    if (unixTime <= 0) {
      return neverPlayedLabel;
    }
    const seconds = Math.floor((Date.now() - unixTime * 1000) / 1000);

    const intervals = {
      [this.translationService.translate('common.year')]: 31536000,
      [this.translationService.translate('common.month')]: 2592000,
      [this.translationService.translate('common.week')]: 604800,
      [this.translationService.translate('common.day')]: 86400,
      [this.translationService.translate('common.hour')]: 3600,
      [this.translationService.translate('common.minute')]: 60
    };

    for (const [unit, secondsInUnit] of Object.entries(intervals)) {
      const interval = Math.floor(seconds / secondsInUnit);
      if (interval >= 1) {
        let unitLabel = unit;
        if (interval > 1) {
          if (unit === this.translationService.translate('common.month')) {
            unitLabel = this.translationService.translate('common.months');
          } else {
            unitLabel = `${unit}s`;
          }
        }
        return `${interval} ${unitLabel} ${agoLabel}`;
      }
    }
    return justNowLabel;
  }
}
