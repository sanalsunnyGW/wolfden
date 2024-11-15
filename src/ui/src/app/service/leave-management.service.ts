import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IAddNewLeaveType } from '../interface/Add-New-Leave-Type-Interface';

@Injectable({
  providedIn: 'root'
})
export class LeaveManagementService {

  constructor() {}

  http=inject(HttpClient);


  addNewLeaveType(newType : FormGroup<IAddNewLeaveType>) {
      const id = localStorage.getItem('doctorId')
      return this.http.post<boolean>("https://localhost:7015/api/leave-type",newType.value)
    }
    
  
}
