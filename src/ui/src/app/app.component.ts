import { Component } from '@angular/core';

import { SigninComponent } from "./user/signin/signin.component";
import { UserComponent } from "./user/user.component";

import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { DashboardComponent } from "./dashboard/dashboard.component";
import { SideNavComponent } from './dashboard/dashboard-body/side-nav/side-nav.component';
import { HeaderComponent } from './dashboard/header/header.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ UserComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wolf-den';
}
