import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-detailed-attendence',
  standalone: true,
  imports: [],
  templateUrl: './detailed-attendence.component.html',
  styleUrl: './detailed-attendence.component.scss'
})
export class DetailedAttendenceComponent {

  constructor(private router:Router) {
  }
  

}
