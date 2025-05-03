import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'playTime'
})
export class PlayTimePipe implements PipeTransform {

  transform(unixTime: number): string {
    const hours = Math.floor(unixTime / 60);
    const minutes = unixTime % 60;
    return `${hours} hrs ${minutes} mins`;
  }

}
