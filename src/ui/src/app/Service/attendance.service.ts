import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../Interfaces/idaily-attendance';
import { WeeklyAttendance } from '../Interfaces/iweekly-attendance';


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
  downloadDailyReport(employeeId: number, date: string) {
    const url = `${this.apiURL}/${employeeId}/downloadPdf?date=${date}`;
    return this.http.get(url,{observe:'response',responseType:'blob'}); 
  }
  getWeeklyChart(employeeId:number,startDate:string,endDate:string)
  {
    const url = `${this.apiURL}/${employeeId}/?startDate=${startDate}/?endDate=${endDate}`;
    return this.http.get<WeeklyAttendance>(url);  
  }


}
