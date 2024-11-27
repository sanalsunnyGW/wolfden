import { Component } from '@angular/core';
import { UpdateLeaveSettingsComponent } from './leave-management/update-leave-settings/update-leave-settings.component';
import { LeaveApplicationComponent } from "./leave-management/leave-application/leave-application.component";
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-main',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
