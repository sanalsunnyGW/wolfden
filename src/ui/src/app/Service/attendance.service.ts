import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../Interface/idaily-attendance';
import { environment } from '../../enviornments/environment';
import { IAttendanceSummary } from '../Interface/attendance-summary';
import { IAttendanceData } from '../Interface/attendance-data';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  constructor() { }

  http = inject(HttpClient);

  private baseUrl=environment.apiUrl;

  getAttendanceSummary(employeeId: number, year: number, month: number) {
    return this.http.get<IAttendanceSummary>(`${this.baseUrl}/api/attendance/employee/monthly?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }

  getDailyStatus(employeeId: number, year: number, month: number) {
    return this.http.get<IAttendanceData[]>(`${this.baseUrl}/api/attendance/employee/daily?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }

  getDailyAttendence(employeeId: number, date: string) {
    const url = `${this.baseUrl}/api/attendance/daily-attendance?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get<DailyAttendance>(url);
  }

  downloadDailyReport(employeeId: number, date: string) {
    const url = `${this.baseUrl}/api/attendance/daily-attendance-pdf?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get(url, { observe: 'response', responseType: 'blob' });
  }
}
