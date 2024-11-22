import { Component } from '@angular/core';
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';
import { LeaveApplicationComponent } from "./leave-management/leave-application/leave-application.component";
import { MonthlyReportComponent } from "./attendence-module/monthly-report/monthly-report.component";


@Component({
  selector: 'app-main',
  standalone: true,
  imports: [MonthlyReportComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
