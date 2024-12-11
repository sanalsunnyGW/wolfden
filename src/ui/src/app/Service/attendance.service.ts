import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../interface/idaily-attendance';
import { environment } from '../../enviornments/environment';
import { IAttendanceSummary } from '../interface/attendance-summary';
import { WeeklyAttendance } from '../interface/iweekly-attendance';
import { ICheckAttencdanceClose } from '../interface/check-attendance-close';
import { SubordinatesDetails } from '../interface/subordinates-details';
import { IAttendanceData } from '../interface/attendance-data';
import { AttendanceHistory } from '../interface/attendance-history';
import { MonthlyReports } from '../interface/monthly-report';
import { GetRange } from '../interface/get-range';

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
  getMonthlyReport(previousClosedDate:string,closedDate:string,pageNumber:number,pageSize:number)
  {
    const url = `${this.baseUrl}/api/attendance/all-employees-monthly-report?PreviousClosedDate=${previousClosedDate}&ClosedDate=${closedDate}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<MonthlyReports>(url); 
  }
  checkAttendanceClose(currentDate:string)
  {
    const url = `${this.baseUrl}/api/attendance/check-attendance-close?AttendanceClose=${currentDate}`;
    return this.http.get<ICheckAttencdanceClose>(url); 
  }
  closeAttendance()
  {
    const url = `${this.baseUrl}/api/attendance/close-attendance?IsClosed=true`;
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
    return this.http.get<SubordinatesDetails>(`${this.baseUrl}/api/attendance/subordinates?EmployeeId=${employeeId}`)
  }
  getMonthlyData(employeeId: number, year: number, month: number)
  {
    return this.http.get(`${this.baseUrl}/api/attendance/monthly-pdf?Month=${month}&Year=${year}&EmployeeId=${employeeId}`,{observe:'response',responseType:'blob'})
  }
  getMonthlyHistoryByStatus(employeeId: number, year: number, month: number, attendanceStatusId: number, pageNumber: number, pageSize: number) {
    let url = `${this.baseUrl}/api/attendance/employee/history?EmployeeId=${employeeId}&Year=${year}&Month=${month}&PageNumber=${pageNumber}&PageSize=${pageSize}`;

    if (attendanceStatusId !== 11 && attendanceStatusId !== 0) {
      url += `&AttendanceStatusId=${attendanceStatusId}`;
  }
    return this.http.get<AttendanceHistory>(url);
  }
  getRange()
  {
    return this.http.get<GetRange>(`${this.baseUrl}/api/attendance/get-range`)
  }
}




