import { Component, OnInit } from '@angular/core';
import { ILeaveBalanceList } from '../../../../../interface/leave-balance-list-interface';
import { FormsModule } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';


@Component({
  selector: 'app-leave-dashboard',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './leave-dashboard.component.html',
  styleUrl: './leave-dashboard.component.scss'
})
export class LeaveDashboardComponent implements OnInit {
id:number=1; 
leaveList:ILeaveBalanceList[]=[];

constructor(private leaveManagementService:LeaveManagementService) {}

ngOnInit()
{
  this.leaveManagementService.getLeaveBalance(this.id).subscribe({
    next: (data) => {
      this.leaveList= data; 
        },
    error: (error) => {
        alert(error);   
       }
  })
}
}
