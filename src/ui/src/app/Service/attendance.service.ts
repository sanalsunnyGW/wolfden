import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../interface/idaily-attendance';
import { environment } from '../../enviornments/environment';
import { MonthlyReports } from '../interface/monthly-report';
import { IAttendanceSummary } from '../interface/attendance-summary';
import { IAttendanceData } from '../interface/attendance-data';
import { WeeklyAttendance } from '../interface/iweekly-attendance';
import { ICheckAttencdanceClose } from '../interface/check-attendance-close';
import { SubordinatesDetails } from '../interface/subordinates-details';

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
    return this.http.get<ICheckAttencdanceClose>(url); 
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
  getWeeklyChart(employeeId:number,weekStart:string,weekEnd:string)
  {
    return this.http.get<WeeklyAttendance[]>(`${this.baseUrl}/api/attendance/employee/weekly?EmployeeId=${employeeId}&WeekStart=${weekStart}&WeekEnd=${weekEnd}`)
  }
  getSubOrdinates(employeeId:number)
  {
    return this.http.get<SubordinatesDetails[]>(`${this.baseUrl}/api/attendance/get-subordinates?EmployeeId=${employeeId}`)
  }
}




