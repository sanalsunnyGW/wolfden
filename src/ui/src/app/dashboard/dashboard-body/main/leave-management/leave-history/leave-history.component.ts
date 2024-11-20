import { Component, OnInit, inject } from '@angular/core';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ILeaveRequestHistory } from '../../../../../interface/leave-request-history';
import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';

@Component({
  selector: 'app-leave-history',
  standalone: true,
  imports: [],
  templateUrl: './leave-history.component.html',
  styleUrl: './leave-history.component.scss'
})
export class LeaveHistoryComponent implements OnInit {

  id: number = 1;
  leaveRequestList: ILeaveRequestHistory[] = [];
  leaveManagementService = inject(LeaveManagementService);

  constructor() { }


  ngOnInit(): void {
    this.leaveManagementService.getLeaveRequestHistory(this.id).subscribe({
      next: (data) => {
        this.leaveRequestList = data;
      },
      error: (error) => {
        alert(error);
      }
    })
  }

  requestStatus(leaveRequest: ILeaveRequestHistory):string {

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
