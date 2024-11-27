import { Injectable, inject } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IEmployeeDirectoryWithPagecount } from '../interface/iemployee-directory-with-pagecount';
@Injectable({
  providedIn: 'root'
})

export class WolfDenService {


  private baseUrl = environment.apiUrl;
  public userId: number = 3;

  constructor(private http: HttpClient) { }

  private createHttpParams(params: { [key: string]: any }): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(params).forEach((key) => {
      if (params[key] !== null && params[key] !== undefined) {
        httpParams = httpParams.set(key, params[key].toString());
      }
    });
    return httpParams;
  }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });

  }

  getEmployeeSignUp(employeeCode: number, rfId: string): Observable<{ id: number, status: boolean }> {
    const params = this.createHttpParams({
      EmployeeCode: employeeCode,
      RFId: rfId,
    });
    return this.http.get<{ id: number, status: boolean }>(
      `${this.baseUrl}/api/Employee/sign-up`,
      { headers: this.getHeaders(), params }
    );
  }

  getEmployeeLogin(email: string, password: string): Observable<any> {
    const params = this.createHttpParams({
      Email: email,
      Password: password,
    });
    return this.http.get<any>(
      `${this.baseUrl}/api/Employee/login`,
      { headers: this.getHeaders(), params }

    );
  }

  signIn(data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Employee/employee-update-employee`, data, { headers: this.getHeaders() });
  }

  getAllEmployees(pageNumber: number, pageSize: number, departmentId?: number, employeeName?: string): Observable<IEmployeeDirectoryWithPagecount> {
    const params = this.createHttpParams({
      PageNumber: pageNumber,
      PageSize: pageSize,
      DepartmentID: departmentId,
      EmployeeName: employeeName,
    });
    return this.http.get<IEmployeeDirectoryWithPagecount>(
      `${this.baseUrl}/api/Employee/all`,
      { headers: this.getHeaders(), params }
    );
  }
}
