import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CalendarViewComponent } from "./attendance-module/calendar-view/calendar-view.component";
import { DailyAttendenceComponent } from "./attendance-module/daily-attendence/daily-attendence.component";
import { SubordinatesComponent } from "./attendance-module/subordinates/subordinates.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [RouterOutlet,SubordinatesComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {
}
