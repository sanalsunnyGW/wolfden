import { HttpClient} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { IAddNewLeaveType } from '../interface/add-new-leave-type-interface';
import { FormGroup } from '@angular/forms';
import { ILeaveRequestHistory } from '../interface/leave-request-history';
import { ILeaveBalanceList } from '../interface/leave-balance-list-interface';
import { IGetLeaveTypeIdAndname } from '../interface/get-leave-type-interface';
import { IEditLeaveType } from '../interface/edit-leave-type';

@Injectable({
  providedIn: 'root'
})

export class LeaveManagementService {

  constructor() { }

  private http = inject(HttpClient);

  getLeaveBalance(id: number) {
    return this.http.get<Array<ILeaveBalanceList>>(`https://localhost:7015/api/leave-balance/${id}`);
  }

  getLeaveRequestHistory(id: number) {
    return this.http.get<Array<ILeaveRequestHistory>>(`https://localhost:7015/api/leave-request/${id}`);
  }

  addNewLeaveType(newType: FormGroup<IAddNewLeaveType>) {
    return this.http.post<boolean>("https://localhost:7015/api/leave-type", newType.value)
  }

  editLeaveType(editLeaveType: FormGroup<IEditLeaveType>) {

    return this.http.put('https://localhost:7015/api/leave-type', editLeaveType.value);
  }

  getLeaveTypeIdAndName(){
    return this.http.get<Array<IGetLeaveTypeIdAndname>>("https://localhost:7015/api/leave-type")
  }
}