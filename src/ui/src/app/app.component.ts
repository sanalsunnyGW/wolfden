import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./user/login/login.component";
import { UserComponent } from "./user/user.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { SideNavComponent } from './dashboard/dashboard-body/side-nav/side-nav.component';
import { HeaderComponent } from './dashboard/header/header.component';
import { WeeklyAttendanceComponent } from "./dashboard/dashboard-body/main/attendence-module/weekly-attendance/weekly-attendance.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DashboardComponent, WeeklyAttendanceComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wolf-den';
}
