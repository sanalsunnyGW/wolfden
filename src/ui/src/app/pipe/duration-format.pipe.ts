import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'durationFormat',
  standalone: true
})
export class DurationFormatPipe implements PipeTransform {
  transform(duration: number): string {
    if (duration == null || duration === 0) {
      return ''; 
    }
    const hours = Math.floor(duration / 60); 
    const minutes = duration % 60; 
    return `${hours} h ${minutes} min`;
  }

}
