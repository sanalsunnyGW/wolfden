import { Injectable, inject } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IEmployeeDirectoryDto } from '../Interface/iemployee-directory';
import { IEmployeeDirectoryWithPagecount } from '../Interface/iemployee-directory-with-pagecount';
@Injectable({
  providedIn: 'root'
})

export class WolfDenService {

  private baseUrl=environment.apiUrl;
  public userId : number=3;

  constructor(private http:HttpClient) { }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });

  }

      getEmployeeSignUp(employeeCode: number, rfId: string): Observable<{ id: number, status: boolean }> {
        let params = new HttpParams()
          .set('EmployeeCode', employeeCode.toString())
          .set('RFId', rfId);

        return this.http.get<{ id: number, status: boolean }>(
          `${this.baseUrl}/api/Employee/sign-up`,
          { headers: this.getHeaders(), params }
        );
      }

      getEmployeeLogin(email: string, password: string): Observable<{ id: number, status: boolean }> {
        let params = new HttpParams()
          .set('Email', email)
          .set('Password', password);

        return this.http.get<{ id: number, status: boolean }>(
          `${this.baseUrl}/api/Employee/login`,
          { headers: this.getHeaders(), params }
         
        );
      }

      signIn(data: any): Observable<any> {
        return this.http.put(`${this.baseUrl}/api/Employee/employee-update-employee`, data, { headers: this.getHeaders() });
      }

      getAllEmployees(pageNumber: number, pageSize: number, departmentId?: number, employeeName?: string ): Observable<IEmployeeDirectoryWithPagecount> {
        let params = new HttpParams();
        
        if (departmentId) {
          params = params.append('DepartmentID', departmentId.toString());
        }
        if (employeeName) {
          params = params.append('EmployeeName', employeeName);
        }
        params = params.append('PageNumber', pageNumber.toString());
        params = params.append('PageSize', pageSize.toString());

        return this.http.get<IEmployeeDirectoryWithPagecount>(
          `${this.baseUrl}/api/Employee/all`,
          { headers: this.getHeaders(), params }
        );
      }
}
