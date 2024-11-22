import { Component } from '@angular/core';
import { ProfileComponent } from '../../../profile/profile.component';
import { RouterOutlet } from '@angular/router';
import { CalendarViewComponent } from "./attendance-module/calendar-view/calendar-view.component";
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';
//import { LeaveApplicationComponent } from "./leave-management/leave-application/leave-application.component";


@Component({
  selector: 'app-main',
  standalone: true,
  // imports: [ LeaveApplicationComponent],
  imports: [RouterOutlet],
 // imports: [ LeaveApplicationComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
