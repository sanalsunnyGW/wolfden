import { Component } from '@angular/core';
import { LeaveDashboardComponent } from './leave-management/leave-dashboard/leave-dashboard.component';
import { AddNewLeaveTypeComponent } from './leave-management/add-new-leave-type/add-new-leave-type.component';
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [UpdateLeaveSettingsComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
