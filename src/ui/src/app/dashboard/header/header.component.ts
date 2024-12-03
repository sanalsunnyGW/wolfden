import { Component, DestroyRef, HostListener, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { WolfDenService } from '../../service/wolf-den.service';
import { EmployeeService } from '../../service/employee.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { NotificationModalComponent } from '../../notification-modal/notification-modal.component';
import { INotificationForm } from '../../interface/i-notification-form';
import { LeaveManagementService } from '../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, NotificationModalComponent, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(
    private router: Router,
    public userService: WolfDenService,
    private employeeService: EmployeeService,
    private toastr: ToastrService){}
    isDropdownOpen = false;

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }
  closeNotifications = (): void => {
    this.showNotifications = false;
  
    //to update the count
    this.userService.getNotification(1).subscribe({
      next: (data) => {
        this.notifications = data.filter(notification => notification); 
      },
      error: (err) => {
        console.error('Error fetching notifications:', err);
      },
    });
  };

  // Close clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    //profile dropdown
    const userMenuContainer = document.querySelector('.user-menu-container');
    if (userMenuContainer && !userMenuContainer.contains(event.target as Node)) {
      this.isDropdownOpen = false;
    }
  }



onLogout(){
  this.userService.userId=0;
  //destroy local stored tocken
  localStorage.removeItem('token');
  this.router.navigate(['/user/login']);
  this.toastr.success("Logged out")
}

}


