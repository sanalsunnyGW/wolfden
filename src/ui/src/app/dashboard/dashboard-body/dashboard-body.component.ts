import { Component } from '@angular/core';
import { SideNavComponent } from "./side-nav/side-nav.component";
import { MainComponent } from "./main/main.component";

@Component({
  selector: 'app-dashboard-body',
  standalone: true,
  imports: [SideNavComponent, MainComponent],
  templateUrl: './dashboard-body.component.html',
  styleUrl: './dashboard-body.component.scss'
})
export class DashboardBodyComponent {

}
