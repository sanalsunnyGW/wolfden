import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DailyAttendance } from '../Interface/idaily-attendance';


@Injectable({
  providedIn: 'root'
})
export class AttendanceService {


  constructor() { }

  http = inject(HttpClient);

  getAttendanceSummary(employeeId: number, year: number, month: number) {
    return this.http.get(`https://localhost:7015/api/AttendenceLog/employee/monthly?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }

  getDailyStatus(employeeId: number, year: number, month: number) {
    return this.http.get(`https://localhost:7015/api/AttendenceLog/employee/daily-status?EmployeeId=${employeeId}&Year=${year}&Month=${month}`);
  }

  private apiURL = "https://localhost:7015/api/attendance"

  getDailyAttendence(employeeId: number, date: string) {
    const url = `${this.apiURL}/daily-attendance?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get<DailyAttendance>(url);
  }
  downloadDailyReport(employeeId: number, date: string) {
    const url = `${this.apiURL}/daily-attendance-pdf?EmployeeId=${employeeId}&Date=${date}`;
    return this.http.get(url, { observe: 'response', responseType: 'blob' });
  }
}
