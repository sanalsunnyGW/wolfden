import { FormControl } from '@angular/forms';

export interface IaddHoliday {
  date: FormControl<Date | null>;           
  type: FormControl<number | null>;    
  description: FormControl<string|null>;   
}