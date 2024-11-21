import { Injectable, inject } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IEmployeeDirectoryDto } from '../dashboard/dashboard-body/main/employee-directory/employee-directory-dto';
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
