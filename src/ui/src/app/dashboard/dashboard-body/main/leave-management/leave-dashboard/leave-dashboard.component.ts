import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Inject, inject, OnInit } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ILeaveBalanceList } from '../../../../../interface/leave-balance-list-interface';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { WolfDenService } from '../../../../../service/wolf-den.service';


@Component({
  selector: 'app-leave-dashboard',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './leave-dashboard.component.html',
  styleUrl: './leave-dashboard.component.scss'
})
export class LeaveDashboardComponent implements OnInit {
userService=inject(WolfDenService);
leaveList:ILeaveBalanceList[]=[];
destroyRef=inject(DestroyRef);

constructor(private leaveManagementService:LeaveManagementService) {}

ngOnInit()
{
  this.leaveManagementService.getLeaveBalance(this.userService.userId)
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe((data)=> {
      this.leaveList= data; 
  });
}
}




