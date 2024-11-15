import { Component } from '@angular/core';
import { CalendarViewComponent } from "./attendance-module/calendar-view/calendar-view.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CalendarViewComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
