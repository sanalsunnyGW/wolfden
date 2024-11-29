import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../interface/update-leave-setting';
import { ILeaveBalanceList } from '../interface/leave-balance-list-interface';
import { ILeaveRequestHistoryResponse } from '../interface/leave-request-history';
import { IGetLeaveTypeIdAndname } from '../interface/get-leave-type-interface';
import { ISubordinateLeaveRequest } from '../interface/subordinate-leave-request';
import { LeaveRequestStatus } from '../enum/leave-request-status-enum';
import { IApproveRejectLeave } from '../interface/approve-or-reject-leave-interface';
import { IEditleave } from '../interface/edit-leave-application-interface';
import { IAddLeaveByAdminForEmployee } from '../interface/add-leave-by-admin-for-employee';
import { IEditLeaveType } from '../interface/edit-leave-type';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { environment } from '../../enviornments/environment';
import { IAddNewLeaveType } from '../interface/Add-New-Leave-Type-Interface';
import { ILeaveApplication } from '../interface/Leave-Application-Interface';

@Injectable({
  providedIn: 'root'
})

export class LeaveManagementService {

  constructor() { }
  private http = inject(HttpClient);
  private baseUrl = environment.leave;
  getLeaveBalance(id: number) {
    return this.http.get<Array<ILeaveBalanceList>>(`${this.baseUrl}/leave-balance?EmployeeId=${id}`);
  }

  getLeaveRequestHistory(id: number, pageNumber: number, pageSize: number): Observable<ILeaveRequestHistoryResponse> {
    return this.http.get<ILeaveRequestHistoryResponse>(`${this.baseUrl}/leave-request?EmployeeId=${id}&PageNumber=${pageNumber}&PageSize=${pageSize}`); 
  }

    addNewLeaveType(newType : IAddNewLeaveType) {
      newType.adminId = this.id;
      return this.http.post<boolean>(`${this.baseUrl}/leave-type`,newType)
    }

    getLeaveSetting(){
      return this.http.get<ILeaveUpdate>(`${this.baseUrl}/leave-setting`)
    }


    updateLeaveSettings(updateLeaveSettings : IUpdateLeaveSetting){
      updateLeaveSettings.adminId=this.id;
      return this.http.put<boolean>(`${this.baseUrl}/leave-setting`,updateLeaveSettings)
    }

    getLeaveTypeIdAndName(){
      return this.http.get<Array<IGetLeaveTypeIdAndname>>(`${this.baseUrl}/leave-type`)
    }

 
    applyLeaveRequest(leaveApplication : ILeaveApplication){
      leaveApplication.empId = this.id;
      return this.http.post<boolean>(`${this.baseUrl}/leave-request`,leaveApplication)
    }

    id : number=1;
    getSubordinateLeaverequest(status :LeaveRequestStatus){
      return this.http.get<Array<ISubordinateLeaveRequest>>(`${this.baseUrl}/leave-request/subordinate-leave-requests/${this.id}/${status}`)
    }
    
    approveOrRejectLeave(approveRejectLeave : IApproveRejectLeave){
      return this.http.patch<boolean>(`${this.baseUrl}/leave-request/subordinate-leave-requests/${this.id}`,approveRejectLeave)
    }

    editLeaveRequest(editleave : IEditleave)
    {
      return this.http.put<boolean>(`${this.baseUrl}/leave-request/edit-leave/${this.id}`,editleave)
    }

    applyLeaveByAdminforEmployee(leaveByAdminforEmployee : IAddLeaveByAdminForEmployee){
      leaveByAdminforEmployee.adminId = this.id;
      return this.http.post<boolean>(`${this.baseUrl}/leave-request/leave-for-employee-by-admin`,leaveByAdminforEmployee)
    } 
    

    editLeaveType(editLeaveType: FormGroup<IEditLeaveType>) {
      return this.http.put(`${this.baseUrl}/leave-type`, editLeaveType.value);
    }

    updateLeaveBalance() {
      return this.http.put<boolean>(`${this.baseUrl}/leave-balance`, null);
    }
}