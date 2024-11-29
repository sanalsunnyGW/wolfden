import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { WolfDenService } from './wolf-den.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }
  employeeId = 1
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
    return this.http.put(`${this.baseUrl}/employee-update-employee`, userForm);
  }
  getMyTeamHierarchy(getFullHierarchy: boolean, employeeId: number) {
    return this.http.get(`${this.baseUrl}/team?Id=${employeeId}&Hierarchy=${getFullHierarchy}`);
  }
  addDepartment(departmentForm: any) {
    return this.http.post(`https://localhost:7015/api/Department`, departmentForm);
  }
  addDesignation(designationForm: any) {
    return this.http.post(`https://localhost:7015/api/Designation`, designationForm);
  }
  getAllDesignation() {
    return this.http.get(`https://localhost:7015/api/Designation/all`);

  }
  getAllDepartment() {
    return this.http.get(`https://localhost:7015/api/Department`);

  }
  getEmployeeByName(firstName: any, lastName?: any) {
    return this.http.get(`https://localhost:7015/api/Employee/get-all-by-name?FirstName=${firstName}&LastName=${lastName}`)
  }
  adminUpdateEmployee(userForm: any) {
    return this.http.put(`https://localhost:7015/api/Employee/admin`, userForm)
  }
  roleChange(roleForm: any) {
    return this.http.put(`https://localhost:7015/api/Employee/super-admin`, roleForm)
  }
}