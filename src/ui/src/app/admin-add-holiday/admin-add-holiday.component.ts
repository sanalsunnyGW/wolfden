import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IaddHoliday } from '../interface/iadd-holiday';

@Component({
  selector: 'app-admin-add-holiday',
  standalone: true,
  imports: [],
  templateUrl: './admin-add-holiday.component.html',
  styleUrl: './admin-add-holiday.component.scss'
})
export class AdminAddHolidayComponent {
  userForm!: FormGroup;
  

}
