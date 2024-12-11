import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { IEmployeeData } from '../interface/employee-data';
import { Employee } from '../interface/iemployee';
import { IDesignation } from '../interface/idesignation';
import { IDepartment } from '../interface/idepartment';
import { ImanagerData } from '../interface/imanager-data';
import { IteamLeave } from '../interface/iteam-leave';
import { IaddEmployee } from '../interface/iadd-employee';
import { IEmployeeUpdate } from '../interface/iemployee-update';
import { IRole } from '../interface/irole-form';
import { IadminUpdate } from '../interface/iadmin-update';
import { IDepartmentData } from '../interface/idepartment-form';
import { IDesignationData } from '../interface/idesignation-form';

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
    return this.http.get<IEmployeeData>(`${this.baseUrl}/employee/hierarchy`);
  }
  getEmployeeProfile(employeeId: number) {
    return this.http.get<Employee>(`${this.baseUrl}/employee/by-Id?EmployeeId=${employeeId}`);
  }
  employeeUpdateEmployee(userForm: IEmployeeUpdate) {
    return this.http.put<boolean>(`${this.baseUrl}/employee/employee-update-employee`, userForm);
  }
  getMyTeamHierarchy(getFullHierarchy: boolean, employeeId: number) {
    return this.http.get<IEmployeeData[]>(`${this.baseUrl}/employee/team?Id=${employeeId}&Hierarchy=${getFullHierarchy}`);
  }
  addDepartment(departmentForm: IDepartmentData) {
    return this.http.post<number>(`${this.baseUrl}/department`, departmentForm);
  }
  addDesignation(designationForm: IDesignationData) {
    return this.http.post<number>(`${this.baseUrl}/designation`, designationForm);
  }
  getAllDesignation() {
    return this.http.get<IDesignation[]>(`${this.baseUrl}/designation`);

  }
  getAllDepartment() {
    return this.http.get<IDepartment[]>(`${this.baseUrl}/department`);

  }
  getEmployeeByName(firstName: string, lastName?: string) {
    return this.http.get<ImanagerData[]>(`${this.baseUrl}/employee/get-all-by-name?FirstName=${firstName}&LastName=${lastName}`)
  }
  adminUpdateEmployee(user: IadminUpdate) {
    return this.http.put<boolean>(`${this.baseUrl}/employee/admin`, user)
  }
  roleChange(role: IRole) {
    return this.http.put<boolean>(`${this.baseUrl}/employee/role`, role)
  }
  addEmployee(employee: IaddEmployee) {
    return this.http.post<number>(`${this.baseUrl}/employee`, employee)
  }
  syncEmployee() {
    return this.http.patch<boolean>(`${this.baseUrl}/employee/employee-sync`, null)
  }
  myTeamLeave(employeeId: number) {
    return this.http.get<IteamLeave[]>(`${this.baseUrl}/leave-request/next-week/approved?EmployeeId=${employeeId}`)
  }
}