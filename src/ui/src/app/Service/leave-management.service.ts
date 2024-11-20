import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../interface/update-leave-setting';
import { IAddNewLeaveType } from '../interface/add-new-leave-type-interface';
import { ILeaveBalanceList } from '../interface/leave-balance-list-interface';
import { ILeaveRequestHistory } from '../interface/leave-request-history';
import { IGetLeaveTypeIdAndname } from '../interface/get-leave-type-interface';
import { IEditLeaveType } from '../interface/edit-leave-type';

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

    addNewLeaveType(newType : FormGroup<IAddNewLeaveType>) {
      return this.http.post<boolean>("https://localhost:7015/api/leave-type",newType.value)
    }

    getLeaveSetting(){
      return this.http.get<ILeaveUpdate>("https://localhost:7015/api/leave-setting")
    }

    updateLeaveSettings(updateLeaveSettings : FormGroup<IUpdateLeaveSetting> ){
      return this.http.put<boolean>("https://localhost:7015/api/leave-setting",updateLeaveSettings.value)
    }

    getLeaveTypeIdAndName(){
      return this.http.get<Array<IGetLeaveTypeIdAndname>>("https://localhost:7015/api/leave-type")
    }

    editLeaveType(editLeaveType: FormGroup<IEditLeaveType>) {
      return this.http.put('https://localhost:7015/api/leave-type', editLeaveType.value);
    }
}