import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';

import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';
import { ILeaveRequestHistory } from '../../../../../Interface/leave-request-history';
import { LeaveManagementService } from '../../../../../service/leave-management.service';



@Component({
  selector: 'app-leave-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './leave-history.component.html',
  styleUrl: './leave-history.component.scss'
})
export class LeaveHistoryComponent {

id:number=1;
leaveRequestList:ILeaveRequestHistory[]=[];
constructor() {}  

leaveManagementService=inject(LeaveManagementService);
ngOnInit(): void {
 
  this.leaveManagementService.getLeaveRequestHistory(this.id).subscribe({
    next: (data) => {
        },
    error: (error) => {
    }
  })
}

RequestStatus(leaveRequest: ILeaveRequestHistory): string {
  switch (leaveRequest.leaveRequestStatus) {
    case LeaveRequestStatus.Open:
      return 'Open';
    case LeaveRequestStatus.Approved:
      return 'Approved';
    case LeaveRequestStatus.Rejected:
      return 'Rejected';
    case LeaveRequestStatus.Deleted:
      return 'Deleted';
    default:
      return 'Unknown Status';
  }
}
}
