import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CalendarViewComponent } from "./attendance-module/calendar-view/calendar-view.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {
}
