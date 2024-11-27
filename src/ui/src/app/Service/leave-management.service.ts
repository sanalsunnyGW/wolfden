import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../interface/update-leave-setting';
import { IAddNewLeaveType } from '../interface/add-new-leave-type-interface';
import { ILeaveBalanceList } from '../interface/leave-balance-list-interface';
import { ILeaveRequestHistory } from '../interface/leave-request-history';
import { IGetLeaveTypeIdAndname } from '../interface/get-leave-type-interface';
import { ILeaveApplication } from '../interface/leave-application-interface';
import { ISubordinateLeaveRequest } from '../interface/subordinate-leave-request';
import { LeaveRequestStatus } from '../enum/leave-request-status-enum';
import { IApproveRejectLeave } from '../interface/approve-or-reject-leave-interface';
import { IEditleave } from '../interface/edit-leave-application-interface';
import { IAddLeaveByAdminForEmployee } from '../interface/add-leave-by-admin-for-employee';

@Injectable({
  providedIn: 'root'
})

export class LeaveManagementService {

  constructor() {}
  private http=inject(HttpClient);
    getLeaveBalance(id:number)
    {
       return this.http.get<Array<ILeaveBalanceList>>(`https://localhost:7015/api/leave-balance/${id}`);
    }

    getLeaveRequestHistory(id:number)
    {
     return this.http.get<Array<ILeaveRequestHistory>>(`https://localhost:7015/api/leave-request/${id}`);
    }

    addNewLeaveType(newType : IAddNewLeaveType) {
      newType.adminId = this.id;
      return this.http.post<boolean>("https://localhost:7015/api/leave-type",newType)
    }

    getLeaveSetting(){
      return this.http.get<ILeaveUpdate>("https://localhost:7015/api/leave-setting")

    }

    updateLeaveSettings(updateLeaveSettings : IUpdateLeaveSetting){
      updateLeaveSettings.adminId=this.id;
      return this.http.put<boolean>("https://localhost:7015/api/leave-setting",updateLeaveSettings)
    }

    getLeaveTypeIdAndName(){
      return this.http.get<Array<IGetLeaveTypeIdAndname>>("https://localhost:7015/api/leave-type")
    }

 
    applyLeaveRequest(leaveApplication : ILeaveApplication){
      leaveApplication.empId = this.id;
      return this.http.post<boolean>(`https://localhost:7015/api/leave-request`,leaveApplication)
    }

    id : number=1;
    getSubordinateLeaverequest(status :LeaveRequestStatus){
      return this.http.get<Array<ISubordinateLeaveRequest>>(`https://localhost:7015/api/leave-request/subordinate-leave-requets${this.id}/${status}`)
    }
    
    approveOrRejectLeave(approveRejectLeave : IApproveRejectLeave){
      return this.http.patch<boolean>(`https://localhost:7015/api/leave-request/subordinate-leave-requets/${this.id}`,approveRejectLeave)
    }

    editLeaveRequest(editleave : IEditleave)
    {
      return this.http.patch<boolean>(`https://localhost:7015/api/leave-request/edit-leave/${this.id}`,editleave)
    }

    applyLeaveByAdminforEmployee(leaveByAdminforEmployee : IAddLeaveByAdminForEmployee){
      leaveByAdminforEmployee.adminId = this.id;
      return this.http.post<boolean>(`https://localhost:7015/api/leave-request/leave-for-employee-by-admin`,leaveByAdminforEmployee)
    } 
    
}