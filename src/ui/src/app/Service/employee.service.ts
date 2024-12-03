import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { WolfDenService } from './wolf-den.service';
import { LoginComponent } from '../user/login/login.component';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }
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
    return this.http.get(`${this.baseUrl}/Employee/hierarchy`);
  }
  getEmployeeProfile(employeeId: number) {
    return this.http.get(`${this.baseUrl}/Employee/by-Id?EmployeeId=${employeeId}`);
  }
  employeeUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/Employee/employee-update-employee`, userForm);
  }
  getMyTeamHierarchy(getFullHierarchy: boolean, employeeId: number) {
    return this.http.get(`${this.baseUrl}/Employee/team?Id=${employeeId}&Hierarchy=${getFullHierarchy}`);
  }
  addDepartment(departmentForm: any) {
    return this.http.post(`${this.baseUrl}/Department`, departmentForm);
  }
  addDesignation(designationForm: any) {
    return this.http.post(`${this.baseUrl}/Designation`, designationForm);
  }
  getAllDesignation() {
    return this.http.get(`${this.baseUrl}/Designation`);

  }
  getAllDepartment() {
    return this.http.get(`${this.baseUrl}/Department`);

  }
  getEmployeeByName(firstName: any, lastName?: any) {
    return this.http.get(`${this.baseUrl}/Employee/get-all-by-name?FirstName=${firstName}&LastName=${lastName}`)
  }
  adminUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/Employee/admin`, userForm)
  }
  roleChange(roleForm: any) {
    return this.http.put(`${this.baseUrl}/Employee/role`, roleForm)
  }
  addEmployee(employeeForm:any){
    return this .http.post(`${this.baseUrl}/Employee`,employeeForm)
  }
  syncEmployee(){
    return this.http.patch(`${this.baseUrl}/Employee/employee-sync`,null)
  }
}