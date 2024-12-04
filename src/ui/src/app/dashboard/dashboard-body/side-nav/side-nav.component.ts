import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { WolfDenService } from '../../../service/wolf-den.service';
import { EmployeeService } from '../../../service/employee.service';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss'
})
export class SideNavComponent {
  userService=inject(WolfDenService)
  employeeService=inject(EmployeeService)


}
