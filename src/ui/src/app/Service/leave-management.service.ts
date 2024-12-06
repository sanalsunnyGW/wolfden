import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ILeaveBalanceList } from '../interface/leave-balance-list-interface';
import { ILeaveRequestHistoryResponse } from '../interface/leave-request-history';
import { IGetLeaveTypeIdAndname } from '../interface/get-leave-type-interface';
import { ILeaveApplication } from '../interface/Leave-Application-Interface';
import { IApproveRejectLeave } from '../interface/approve-or-reject-leave-interface';
import { IEditleave } from '../interface/edit-leave-application-interface';
import { environment } from '../../enviornments/environment';
import { ISubordinateLeavePaginationSend } from '../interface/subordinate-leave-request-pagination-send';
import { ISubordinateLeavePaginationReceive } from '../interface/subordinate-leave-request-pagination-receive';
import { WolfDenService } from './wolf-den.service';
import { IRevokeLeave } from '../interface/revoke-leave';
import { IAddNewLeaveType } from '../interface/Add-New-Leave-Type-Interface';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../interface/update-leave-setting';
import { IAddLeaveByAdminForEmployee } from '../interface/add-leave-by-admin-for-employee';
import { IEditLeaveType} from '../interface/edit-leave-type';
import { ILeaveRequest } from '../interface/leave-request';

@Injectable({
  providedIn: 'root'
})

export class LeaveManagementService {

  constructor() { }
  private http = inject(HttpClient);
  private userService= inject(WolfDenService);
  private baseUrl = environment.leave;
  
  getLeaveBalance(id: number) {
    return this.http.get<Array<ILeaveBalanceList>>(`${this.baseUrl}/leave-balance?EmployeeId=${id}`);
  }

  getLeaveRequestHistory(id: number, pageNumber: number, pageSize: number, selectedStatus: number|null) {
    console.log(this.userService.userId)
    console.log(id)
    if (selectedStatus!=null) {
      return this.http.get<ILeaveRequestHistoryResponse>(`${this.baseUrl}/leave-request?EmployeeId=${id}&PageNumber=${pageNumber}&PageSize=${pageSize}&LeaveStatusId=${selectedStatus}`);
    }
    else {
      return this.http.get<ILeaveRequestHistoryResponse>(`${this.baseUrl}/leave-request?EmployeeId=${id}&PageNumber=${pageNumber}&PageSize=${pageSize}`);
    }
  }

    addNewLeaveType(newType : IAddNewLeaveType) {
      return this.http.post<boolean>(`${this.baseUrl}/leave-type`,newType)
    }

    getLeaveSetting(){
      return this.http.get<ILeaveUpdate>(`${this.baseUrl}/leave-setting`)
    }


    updateLeaveSettings(updateLeaveSettings : IUpdateLeaveSetting){
      updateLeaveSettings.adminId = this.userService.userId;
      return this.http.put<boolean>(`${this.baseUrl}/leave-setting`,updateLeaveSettings)
    }

    getLeaveTypeIdAndName(){
      return this.http.get<Array<IGetLeaveTypeIdAndname>>(`${this.baseUrl}/leave-type`)
    }

    applyLeaveRequest(leaveApplication : ILeaveApplication){
      leaveApplication.empId = this.userService.userId;
      return this.http.post<ILeaveRequest>(`${this.baseUrl}/leave-request`,leaveApplication)
    }

    getSubordinateLeaverequest(pagination :ISubordinateLeavePaginationSend){
      pagination.id = this.userService.userId
      return this.http.get<ISubordinateLeavePaginationReceive>(`${this.baseUrl}/leave-request/subordinate-leave-requests?Id=${pagination.id}&StatusId=${pagination.statusId}&PageSize=${pagination.pageSize}&PageNumber=${pagination.pageNumber}`)
    }
    
    approveOrRejectLeave(approveRejectLeave : IApproveRejectLeave){
      return this.http.patch<boolean>(`${this.baseUrl}/leave-request/subordinate-leave-requests/${this.userService.userId}`,approveRejectLeave)
    }

    editLeaveRequest(editleave : IEditleave)
    {
      return this.http.put<ILeaveRequest>(`${this.baseUrl}/leave-request/edit-leave/${this.userService.userId}`,editleave)
    }

    applyLeaveByAdminforEmployee(leaveByAdminforEmployee : IAddLeaveByAdminForEmployee){
      leaveByAdminforEmployee.adminId = this.userService.userId
      return this.http.post<ILeaveRequest>(`${this.baseUrl}/leave-request/leave-for-employee-by-admin`,leaveByAdminforEmployee)
    } 
    
    editLeaveType(editLeaveType: IEditLeaveType) {
      return this.http.put<boolean>(`${this.baseUrl}/leave-type`, editLeaveType);
    }

    updateLeaveBalance() {
      return this.http.put<boolean>(`${this.baseUrl}/leave-balance`, null);
    }

    revokeLeaveRequest(leaveRequestId : IRevokeLeave){
      return this.http.patch<boolean>(`${this.baseUrl}/leave-request/revoke-leave/${this.userService.userId}`,leaveRequestId);
    }

    getLeaveDetails(typeId:number){
    return this.http.get<IEditLeaveType>(`${this.baseUrl}/leave-type/details?RequestId=${typeId}`);
    }

    getLeaveRequest(leaveRequestId : number)
    {
      return this.http.get<IEditleave>(`${this.baseUrl}/leave-request/${leaveRequestId}`)
    }
}