import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./user/login/login.component";
import { UserComponent } from "./user/user.component";
import { LeaveDashboardComponent } from './Leave-Management/leave-dashboard/leave-dashboard.component';
import { DashboardComponent } from "./dashboard/dashboard.component";
import { SideNavComponent } from './dashboard/dashboard-body/side-nav/side-nav.component';
import { HeaderComponent } from './dashboard/header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DashboardComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wolf-den';
}
