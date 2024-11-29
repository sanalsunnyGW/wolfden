import { Component, DestroyRef, Inject, inject, OnInit } from '@angular/core';
import { LeaveRequestStatus } from '../../../../../enum/leave-request-status-enum';
import { IApproveRejectLeave } from '../../../../../interface/approve-or-reject-leave-interface';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ISubordinateLeavePaginationSend } from '../../../../../interface/subordinate-leave-request-pagination-send';
import { ISubordinateLeavePaginationReceive } from '../../../../../interface/subordinate-leave-request-pagination-receive';
import { ISubordinateLeaveRequest } from '../../../../../interface/subordinate-leave-request';
import { LeaveManagementService } from '../../../../../service/leave-management.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { pipe } from 'rxjs';

@Component({
  selector: 'app-subordinate-leave-request',
  standalone: true,
  imports: [MatPaginatorModule],
  templateUrl: './subordinate-leave-request.component.html',
  styleUrl: './subordinate-leave-request.component.scss'
})
export class SubordinateLeaveRequestComponent implements OnInit{

  leavestatus : LeaveRequestStatus= LeaveRequestStatus.Open
  open :  LeaveRequestStatus =LeaveRequestStatus.Open
  approved : LeaveRequestStatus = LeaveRequestStatus.Approved
  rejected : LeaveRequestStatus= LeaveRequestStatus.Rejected
  leaveRequestList : ISubordinateLeaveRequest[] = []
  leaveRequest : ISubordinateLeaveRequest = {} as ISubordinateLeaveRequest
  selectedDescription: string | null = null;
  pageNumber= 0;
  pageSize=3;
  pageSizes=[3,5,10]
  totalDataCount : number | null = null
  indexValue: number = (this.pageNumber * this.pageSize) - this.pageSize + 1;
  pagination : ISubordinateLeavePaginationSend ={} as ISubordinateLeavePaginationSend

  approveRejectLeave : IApproveRejectLeave = {} as IApproveRejectLeave

  leaveManagementService = inject(LeaveManagementService);
  destroyRef= Inject(DestroyRef);


  loadLeaveRequests(filterStatus : LeaveRequestStatus, pageNumber : number, pageSize : number): void {
    this.pagination.statusId = filterStatus
    this.pagination.pageNumber = pageNumber
    this.pagination.pageSize = pageSize
    this.leaveManagementService.getSubordinateLeaverequest(this.pagination).
    pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (data: ISubordinateLeavePaginationReceive) => {
        if(data){
          this.leaveRequestList = data.subordinateLeaveDtosList;
          this.totalDataCount = data.totalDataCount
          this.indexValue = ((this.pageNumber+1 ) * this.pageSize) - this.pageSize + 1;
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
    this.leavestatus = parseInt(selectedValue)
    this.loadLeaveRequests(parseInt(selectedValue),this.pageNumber,this.pageSize); 
  }
  
  ngOnInit(): void {
    this.loadLeaveRequests(this.open,this.pageNumber,this.pageSize);
  }

  approve(id :number){
    this.approveRejectLeave.leaveRequestId = id;
    this.approveRejectLeave.statusId = LeaveRequestStatus.Approved;
    this.leaveManagementService.approveOrRejectLeave(this.approveRejectLeave)
    .pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (response: boolean) => {
        if(response){
          alert("Leave Approved");
          this.loadLeaveRequests(this.leavestatus,this.pageNumber,this.pageSize);
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
          this.loadLeaveRequests(this.leavestatus,this.pageNumber,this.pageSize);
        }
       
        },
        error: (error) => {
          alert(error);

      }
    });
  }

  onPaginateChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;  
    this.pageSize = event.pageSize;
    this.loadLeaveRequests(this.leavestatus,this.pageNumber,this.pageSize);
  }

}
