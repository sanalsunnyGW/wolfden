import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ILeaveRequestHistory } from '../../../../../interface/leave-request-history';

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
      console.log('initially Fetched Leave Request list:', data); 
      this.leaveRequestList= data; 
        },
    error: (error) => {
      console.log(error);
    }
  })
}

RequestStatus(leaveRequest:ILeaveRequestHistory) {
 let requestStatus:String='';

  if(leaveRequest.leaveRequestStatus==1)
{
   requestStatus='Open';
  }
  else if(leaveRequest.leaveRequestStatus==2)
  {
   requestStatus='Approved'; 
  }
  else if(leaveRequest.leaveRequestStatus==3)
  {
    requestStatus='Rejected';
  }
  else if(leaveRequest.leaveRequestStatus==4)
  {
    requestStatus='Deleted';
  }
  return requestStatus;
}
}
