import { Component } from '@angular/core';
import { EmployeeDirectoryComponent } from "./employee-directory/employee-directory.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [EmployeeDirectoryComponent],
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
