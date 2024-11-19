import { Component } from '@angular/core';
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';
import { LeaveApplicationComponent } from "./leave-management/leave-application/leave-application.component";
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
