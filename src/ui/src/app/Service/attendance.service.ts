import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { WeeklyAttendance } from '../interface/iweekly-attendance';
import { DailyAttendance } from '../interface/idaily-attendance';


@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  constructor() { }
  http=inject(HttpClient)

  private apiURL="https://localhost:7015/api/attendance"
  
  getDailyAttendence(employeeId:number,date:string)
  {
    const url = `${this.apiURL}/daily-attendance?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get<DailyAttendance>(url);  
  }
  downloadDailyReport(employeeId: number, date: string) {
    const url = `${this.apiURL}/daily-attendance-pdf?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get(url,{observe:'response',responseType:'blob'}); 
  }
}
