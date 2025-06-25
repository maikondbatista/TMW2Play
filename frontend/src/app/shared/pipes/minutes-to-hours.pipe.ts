import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'minutesToHours',
})
export class MinutesToHoursPipe implements PipeTransform {
  transform(minutes: number): number {
    const hours = minutes / 60;
    if (hours >= 10) {
      return Math.round(hours);
    } else {
      return Math.round(hours * 10) / 10;
    }
  }
}