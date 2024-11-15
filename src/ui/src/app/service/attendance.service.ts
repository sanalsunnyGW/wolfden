import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../interface/daily-attendance';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  constructor() { }
  http=inject(HttpClient)

  private apiURL="https://localhost:7015/api/attendance"
  
  getDailyAttendence(employeeId:number,date:string)
  {
    const url = `${this.apiURL}/${employeeId}/?date=${date}`;
    return this.http.get<DailyAttendance>(url);  
  }

}
