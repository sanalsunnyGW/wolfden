import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'splitComma',
  standalone: true
})
export class SplitCommaPipe implements PipeTransform {

  transform(value: string): string[] {
    if (!value) return [];
    return value.split(',').map(val => val.trim());
  }

}
