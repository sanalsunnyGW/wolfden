import { Component, HostListener, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../service/wolf-den.service';
import { EmployeeService } from '../../service/employee.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  loginRole: string = '';


  constructor(
    private router: Router,
    public userService: WolfDenService,
    private employeeService: EmployeeService,
    private wolfdenService: WolfDenService,
    private toastr: ToastrService) {
    const login = employeeService.decodeToken();
    this.loginRole =wolfdenService.role;
  }
  isDropdownOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  // Close clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const userMenuContainer = document.querySelector('.user-menu-container');
    if (userMenuContainer && !userMenuContainer.contains(event.target as Node)) {
      this.isDropdownOpen = false;
    }
  }



  onLogout() {
    this.userService.userId = 0;
    //destroy local stored tocken
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
    this.toastr.success("Logged out")
  }

}
