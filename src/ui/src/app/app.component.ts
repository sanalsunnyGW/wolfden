import { Component } from '@angular/core';
import { DashboardComponent } from "./dashboard/dashboard.component";


import { SideNavComponent } from './dashboard/dashboard-body/side-nav/side-nav.component';
import { HeaderComponent } from './dashboard/header/header.component';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'wolf-den';
}
