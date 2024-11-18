import { Component } from '@angular/core';
import { DailyAttendenceComponent } from "./attendence-module/daily-attendence/daily-attendence.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [DailyAttendenceComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
