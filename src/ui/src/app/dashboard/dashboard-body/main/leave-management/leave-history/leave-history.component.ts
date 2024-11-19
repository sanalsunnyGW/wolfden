import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ILeaveRequestHistory } from '../../../../../interface/leave-request-history';
import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';

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

/**
 *
 */
constructor() {}  

leaveManagementService=inject(LeaveManagementService);
ngOnInit(): void {
  //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
  //Add 'implements OnInit' to the class.
  this.leaveManagementService.getLeaveRequestHistory(this.id).subscribe({
    next: (data) => {
        },
    error: (error) => {
      console.log(error);
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
