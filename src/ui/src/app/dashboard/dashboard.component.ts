import { Component } from '@angular/core';
import { MainComponent } from './main/main.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { HeaderComponent } from './header/header.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MainComponent,SideNavComponent,HeaderComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
