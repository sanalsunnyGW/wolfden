import { Injectable, inject } from '@angular/core';
import { environment } from '../enviornments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import {  IEmployeeDirectoryDto } from './dashboard/dashboard-body/main/employee-directory/employee-directory-dto';

@Injectable({
  providedIn: 'root'
})

export class WolfDenService {

  private baseUrl=environment.apiUrl;
  public userId : number=1;

  constructor(private http:HttpClient) { }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });

  }

  signIn(data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Employee/EmployeeUpdateEmployee`, data, { headers: this.getHeaders() });
  }

  getAllEmployees(departmentId?: number, employeeName?: string): Observable<IEmployeeDirectoryDto[]> {
    let params = new HttpParams();
    
    if (departmentId) {
      params = params.append('DepartmentID', departmentId.toString());
    }
    if (employeeName) {
      params = params.append('EmployeeName', employeeName);
    }

    return this.http.get<IEmployeeDirectoryDto[]>(
      `${this.baseUrl}/api/Employee/all`,
      { headers: this.getHeaders(), params }
    );
  }
}
