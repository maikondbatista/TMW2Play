import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'minutesToHours',
})
export class MinutesToHoursPipe implements PipeTransform {
  transform(minutes: number): number {
    return minutes / 60;
  }
}