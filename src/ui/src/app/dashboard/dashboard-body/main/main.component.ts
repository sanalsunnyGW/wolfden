import { Component } from '@angular/core';
import { AddNewLeaveTypeComponent } from './leave-management/add-new-leave-type/add-new-leave-type.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [AddNewLeaveTypeComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
