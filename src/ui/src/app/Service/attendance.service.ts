import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  

  constructor() { }

  http=inject(HttpClient);

  getAttendanceSummary(employeeId: number, year: number, month: number) {
    return this.http.get(`https://localhost:7015/api/AttendenceLog/employee/${1}/monthly?year=${year}&month=${month}`);
  }

  getDailyStatus(employeeId: number, year: number, month: number){
    return this.http.get(`https://localhost:7015/api/AttendenceLog/employee/${1}/dailystatus?year=${year}&month=${month}`);
  }

  
  
}