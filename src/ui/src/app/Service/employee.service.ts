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
    return this.http.get(`${this.baseUrl}/employee/hierarchy`);
  }
  getEmployeeProfile(employeeId: number) {
    return this.http.get(`${this.baseUrl}/employee/by-Id?EmployeeId=${employeeId}`);
  }
  employeeUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/employee/employee-update-employee`, userForm);
  }
  getMyTeamHierarchy(getFullHierarchy: boolean, employeeId: number) {
    return this.http.get(`${this.baseUrl}/employee/team?Id=${employeeId}&Hierarchy=${getFullHierarchy}`);
  }
  addDepartment(departmentForm: any) {
    return this.http.post(`${this.baseUrl}/department`, departmentForm);
  }
  addDesignation(designationForm: any) {
    return this.http.post(`${this.baseUrl}/designation`, designationForm);
  }
  getAllDesignation() {
    return this.http.get(`${this.baseUrl}/designation`);

  }
  getAllDepartment() {
    return this.http.get(`${this.baseUrl}/department`);

  }
  getEmployeeByName(firstName: any, lastName?: any) {
    return this.http.get(`${this.baseUrl}/employee/get-all-by-name?FirstName=${firstName}&LastName=${lastName}`)
  }
  adminUpdateEmployee(userForm: any) {
    return this.http.put(`${this.baseUrl}/employee/admin`, userForm)
  }
  roleChange(roleForm: any) {
    return this.http.put(`${this.baseUrl}/employee/role`, roleForm)
  }
  addEmployee(employeeForm:any){
    return this .http.post(`${this.baseUrl}/employee`,employeeForm)
  }
  syncEmployee(){
    return this.http.patch(`${this.baseUrl}/employee/employee-sync`,null)
  }
  myTeamLeave(employeeId:number){
    return this.http.get(`${this.baseUrl}/leave-request/next-week/approved?EmployeeId=${employeeId}`)
  }
}