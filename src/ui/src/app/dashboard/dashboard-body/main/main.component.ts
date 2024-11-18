import { Component } from '@angular/core';
import { ProfileComponent } from '../../../profile/profile.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-main',
  standalone: true,
  // imports: [ LeaveApplicationComponent],
  imports: [ProfileComponent,RouterOutlet],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
