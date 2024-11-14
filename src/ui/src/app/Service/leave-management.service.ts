import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
  })

  export class LeaveManagementService {
  
     constructor(private http:HttpClient) {}
   
     getLeaveBalance(id:number):Observable<any>
     {
        const params=new HttpParams().set('RequestId',id);
        return this.http.get('https://localhost:7015/api/LeaveBalance',{params});
     }
  }