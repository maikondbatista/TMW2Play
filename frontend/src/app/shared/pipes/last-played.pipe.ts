import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'lastPlayed'
})
export class LastPlayedPipe implements PipeTransform {

  transform(unixTime: number): string {
    if (unixTime <= 0) {
      return 'Never played';
    }
    const seconds = Math.floor((Date.now() - unixTime * 1000) / 1000);

    const intervals = {
      year: 31536000,
      month: 2592000,
      week: 604800,
      day: 86400,
      hour: 3600,
      minute: 60
    };

    for (const [unit, secondsInUnit] of Object.entries(intervals)) {
      const interval = Math.floor(seconds / secondsInUnit);
      if (interval >= 1) {
        return interval === 1 ? `${interval} ${unit} ago` : `${interval} ${unit}s ago`;
      }
    }
    return 'Just now';
  }

}
