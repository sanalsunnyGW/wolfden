import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../enviornments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }
  employeeId = 1;
  private baseUrl = environment.employeeapiUrl;

  decodeToken() {
    const token = localStorage.getItem('token');
    if (!token) {
      return null;
    }
    try {
      const tokenParts = token.split('.');
      if (tokenParts.length !== 3) {
        return null;
      }
      const payloadBase64 = tokenParts[1];
      const payloadJson = window.atob(payloadBase64);
      const payload = JSON.parse(payloadJson);
      return payload;
    } catch (error) {
      return null;
    }
  }

  getHierarchy() {
    return this.http.get(`${this.baseUrl}/hierarchy`);
  }
  getEmployeeProfile(employeeId: number) {
    return this.http.get(`${this.baseUrl}/by-Id?EmployeeId=${employeeId}`);
  }
  employeeUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/employee-update-employee`, userForm)
  }
  getMyTeamHierarchy(getFullHierarchy: boolean) {
    return this.http.get(`${this.baseUrl}/team?Id=${this.employeeId}&Hierarchy=${getFullHierarchy}`);
  }
}
