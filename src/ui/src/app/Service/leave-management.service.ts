import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

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
  }