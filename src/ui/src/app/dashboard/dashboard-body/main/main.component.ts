import { Component } from '@angular/core';
import { LeaveDashboardComponent } from './leave-management/leave-dashboard/leave-dashboard.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [LeaveDashboardComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
