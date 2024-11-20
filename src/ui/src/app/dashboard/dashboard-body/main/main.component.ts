import { Component } from '@angular/core';
import { EmployeeDirectoryComponent } from "./employee-directory/employee-directory.component";
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';
import { LeaveApplicationComponent } from "./leave-management/leave-application/leave-application.component";


@Component({
  selector: 'app-main',
  standalone: true,
  imports: [],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
