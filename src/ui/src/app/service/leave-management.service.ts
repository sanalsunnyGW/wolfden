import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IAddNewLeaveType } from '../interface/Add-New-Leave-Type-Interface';
import { FormGroup } from '@angular/forms';
import { ILeaveUpdate, IUpdateLeaveSetting } from '../interface/update-leave-setting';

@Injectable({
  providedIn: 'root'
})

export class LeaveManagementService {

  constructor() {}
  http=inject(HttpClient);
    getLeaveBalance(id:number):Observable<any>
    {
       const params=new HttpParams().set('RequestId',id);
       console.log(id);
       return this.http.get('https://localhost:7015/api/leave-balance',{params});
    }

    getLeaveRequestHistory(id:number):Observable<any>
    {
     const params=new HttpParams().set('requestId',id);
     console.log('LeaveRequest History',id);
     return this.http.get('https://localhost:7015/api/leave-request',{params});
    }

    addNewLeaveType(newType : FormGroup<IAddNewLeaveType>) {
      return this.http.post<boolean>("https://localhost:7015/api/leave-type",newType.value)
    }

    getLeaveSetting(){
      console.log("test")
      return this.http.get<ILeaveUpdate>("https://localhost:7015/api/leave-setting")

    }

    updateLeaveSettings(updateLeaveSettings : FormGroup<IUpdateLeaveSetting> ){
      return this.http.put<boolean>("https://localhost:7015/api/leave-setting",updateLeaveSettings.value)
    }
}