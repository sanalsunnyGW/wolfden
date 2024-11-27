import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { ILeaveBalanceList } from '../../../../../Interface/leave-balance-list-interface';
import { FormsModule } from '@angular/forms';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';


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
destroyRef=inject(DestroyRef);

constructor(private leaveManagementService:LeaveManagementService) {}

ngOnInit()
{
  this.leaveManagementService.getLeaveBalance(this.id)
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe((data)=> {
      this.leaveList= data; 
  });
}
}




