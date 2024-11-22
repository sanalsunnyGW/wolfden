import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../interface/idaily-attendance';
import { environment } from '../../enviornments/environment';
import { MonthlyReports } from '../interface/monthly-report';
import { IAttendanceSummary } from '../interface/attendance-summary';
import { IAttendanceData } from '../interface/attendance-data';

@Injectable({
  providedIn: 'root'
})

export class AttendanceService {
  constructor() { }
  http=inject(HttpClient)
  private baseUrl=environment.apiUrl
  
  getDailyAttendence(employeeId:number,date:string)
  {
    const url = `${this.baseUrl}/api/attendance/daily-attendance?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get<DailyAttendance>(url);  
  }
  downloadDailyReport(employeeId: number, date: string) {
    const url = `${this.baseUrl}/api/attendance/daily-attendance-pdf?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get(url,{observe:'response',responseType:'blob'}); 
  }
  getMonthlyReport(month:number,year:number,pageNumber:number,pageSize:number)
  {
    const url = `${this.baseUrl}/api/attendance/all-employees-monthly-report?Month=${month}&Year=${year}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<MonthlyReports>(url); 
  }
  checkAttendanceClose(month:number,year:number)
  {
    const url = `${this.baseUrl}/api/attendance/check-attendance-close?Month=${month}&Year=${year}`;
    return this.http.get<boolean>(url); 
  }
  closeAttendance(month: number, year: number)
  {
    const url = `${this.baseUrl}/api/attendance/close-attendance?Month=${month}&Year=${year}&IsClosed=${true}`;
    return this.http.post(url,null); 
  }
  getAttendanceSummary(employeeId: number, year: number, month: number) {
    return this.http.get<IAttendanceSummary>(`${this.baseUrl}/api/attendance/employee/monthly?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }
  getDailyStatus(employeeId: number, year: number, month: number) {
    return this.http.get<IAttendanceData[]>(`${this.baseUrl}/api/attendance/employee/daily?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }
}




