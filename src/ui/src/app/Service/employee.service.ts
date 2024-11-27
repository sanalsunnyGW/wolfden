import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { WolfDenService } from './wolf-den.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }
  employeeId=8
  private baseUrl = environment.employeeapiUrl;

  getHierarchy() {
    return this.http.get(`${this.baseUrl}/hierarchy`);
  }
  getEmployeeProfile() {
    return this.http.get(`${this.baseUrl}/by-Id?EmployeeId=1`);
  }
  employeeUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/employee-update-employee`, userForm)
  }
  getMyTeamHierarchy(getFullHierarchy: boolean) {
    return this.http.get(`${this.baseUrl}/team?Id=${this.employeeId}&Hierarchy=${getFullHierarchy}`);
  }
}
