import { Component, OnInit } from '@angular/core';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';

@Component({
  selector: 'app-update-leave-balance',
  standalone: true,
  imports: [],
  templateUrl: './update-leave-balance.component.html',
  styleUrl: './update-leave-balance.component.scss'
})
export class UpdateLeaveBalanceComponent implements OnInit {

  constructor(private leaveManagementService: LeaveManagementService) { }

  ngOnInit() {
    this.leaveManagementService.updateLeaveBalance().subscribe({
      next: (data: boolean) => {
        if (data === true) {
          alert('Leave Balance of All employees Updated !!');
        }
        else {
          alert('Sorry ! Encountered some issues while Updating employees leave balance !')
        }
      },
      error: (error) => {
        alert(error);
      }
    })
  }
}
