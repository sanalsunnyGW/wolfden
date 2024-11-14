import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ILeaveBalanceList } from '../../Interface/leave-balance-list-interface';

@Component({
  selector: 'app-leave-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './leave-dashboard.component.html',
  styleUrl: './leave-dashboard.component.scss'
})
export class LeaveDashboardComponent {
id:number=6; //1-for hr and 2-higher user 0-for all user
leaveList:ILeaveBalanceList[]=[];

ngOnit()
{
  this.leaveManagementService.getLeaveBalance(this.userId).subscribe({
    next: (data) => {
      console.log('initially Fetched expenses:', data); 
      this.expenseList = data; 
      this.totalAmount = this.expenseList.reduce((sum, expense) => sum + expense.amount, 0);

    },
    error: (error) => {
      console.log(error);
    }
  })
}
}
