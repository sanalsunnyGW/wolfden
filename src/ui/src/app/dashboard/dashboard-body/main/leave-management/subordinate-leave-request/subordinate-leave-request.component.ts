import { Component, inject } from '@angular/core';
import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';
import { LeaveManagementService } from '../../../../../Service/leave-management.service';
import { ISubordinateLeaveRequest } from '../../../../../interface/subordinate-leave-request';
import { IApproveRejectLeave } from '../../../../../interface/approve-or-reject-leave-interface';

@Component({
  selector: 'app-subordinate-leave-request',
  standalone: true,
  imports: [],
  templateUrl: './subordinate-leave-request.component.html',
  styleUrl: './subordinate-leave-request.component.scss'
})
export class SubordinateLeaveRequestComponent {

  leavestatus : LeaveRequestStatus= LeaveRequestStatus.Open
  open :  LeaveRequestStatus =LeaveRequestStatus.Open
  approved : LeaveRequestStatus = LeaveRequestStatus.Approved
  rejected : LeaveRequestStatus= LeaveRequestStatus.Rejected
  leaveRequestList: ISubordinateLeaveRequest[] = [];
  selectedDescription: string | null = null;
  approveRejectLeave : IApproveRejectLeave = {
    superiorId : 0,
    leaveRequestId : null,
    statusId : null,
  };
  
  leaveManagementService = inject(LeaveManagementService);



  loadLeaveRequests(filterStatus : LeaveRequestStatus): void {
    console.log(filterStatus)
    this.leavestatus = filterStatus
    this.leaveManagementService.getSubordinateLeaverequest(this.leavestatus).subscribe({
      next: (data: Array<ISubordinateLeaveRequest>) => {
        if(data){
          this.leaveRequestList = data;
        }
        
      },
      error: (error) => {
        alert(error);
      }
    });
  }

  viewDescription(description: string): void {
    this.selectedDescription = description;
  }

  closeModal(): void {
    this.selectedDescription = null;
  }

  onFilterChange(event: Event): void {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.loadLeaveRequests(parseInt(selectedValue)); 
  }
  
  ngOnInit(): void {
    this.loadLeaveRequests(this.open);
  }

  approve(id :number){
    this.approveRejectLeave.leaveRequestId = id;
    this.approveRejectLeave.statusId = LeaveRequestStatus.Approved;
    this.leaveManagementService.approveOrRejectLeave(this.approveRejectLeave).subscribe({
      next: (response: boolean) => {
        if(response){
          alert("Leave Approved");
          this.loadLeaveRequests(this.leavestatus);
        }
       
        },
        error: (error) => {
          alert(error);

      }
    });
    
  }

  reject(id :number){
    console.log(id);
    this.approveRejectLeave.leaveRequestId = id;
    this.approveRejectLeave.statusId = LeaveRequestStatus.Rejected;
    this.leaveManagementService.approveOrRejectLeave(this.approveRejectLeave).subscribe({
      next: (response: boolean) => {
        if(response){
          alert("Leave rejected");
          this.loadLeaveRequests(this.leavestatus);
        }
       
        },
        error: (error) => {
          alert(error);

      }
    });
  }
}
