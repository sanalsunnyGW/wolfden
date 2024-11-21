import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';
import { ILeaveBalanceList } from '../../../../../Interface/leave-balance-list-interface';


@Component({
  selector: 'app-leave-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './leave-dashboard.component.html',
  styleUrl: './leave-dashboard.component.scss'
})
export class LeaveDashboardComponent {
id:number=1; //1-for hr and 2-higher user any number-for all user
leaveList:ILeaveBalanceList[]=[];

constructor(private leaveManagementService:LeaveManagementService) {}

ngOnInit()
{
  this.leaveManagementService.getLeaveBalance(this.id).subscribe({
    next: (data) => {
      this.leaveList= data; 
        },
    error: (error) => {
      console.log(error);
    }
  })
}
}
