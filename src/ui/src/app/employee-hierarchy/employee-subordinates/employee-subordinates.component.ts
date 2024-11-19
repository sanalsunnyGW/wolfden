import { Component, Input } from '@angular/core';
import { Employee } from '../iemployee-hierarchy';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-subordinates',
  standalone: true,
  imports: [CommonModule ],
  templateUrl: './employee-subordinates.component.html',
  styleUrl: './employee-subordinates.component.scss'
})
export class EmployeeSubordinatesComponent {
  @Input() employee: Employee | null = null; 


}
