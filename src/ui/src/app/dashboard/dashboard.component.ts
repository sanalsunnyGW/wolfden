import { Component } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { SideNavComponent } from './dashboard-body/side-nav/side-nav.component';
import { DashboardBodyComponent } from "./dashboard-body/dashboard-body.component";
import { RouterLink, RouterLinkActive } from '@angular/router';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DashboardBodyComponent, HeaderComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
