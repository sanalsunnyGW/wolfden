import { Component, DestroyRef, Inject, inject, OnInit } from '@angular/core';
import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { ISubordinateLeaveRequest } from '../../../../../interface/subordinate-leave-request';
import { IApproveRejectLeave } from '../../../../../interface/approve-or-reject-leave-interface';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-subordinate-leave-request',
  standalone: true,
  imports: [],
  templateUrl: './subordinate-leave-request.component.html',
  styleUrl: './subordinate-leave-request.component.scss'
})
export class SubordinateLeaveRequestComponent implements OnInit{

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
  destroyRef= inject(DestroyRef);
  leaveManagementService = inject(LeaveManagementService);



  loadLeaveRequests(filterStatus : LeaveRequestStatus): void {
    this.leavestatus = filterStatus
    this.leaveManagementService.getSubordinateLeaverequest(this.leavestatus)
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
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
    this.leaveManagementService.approveOrRejectLeave(this.approveRejectLeave)
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
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
    this.approveRejectLeave.leaveRequestId = id;
    this.approveRejectLeave.statusId = LeaveRequestStatus.Rejected;
    this.leaveManagementService.approveOrRejectLeave(this.approveRejectLeave)
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
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
