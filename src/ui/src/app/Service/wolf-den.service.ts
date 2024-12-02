import { Injectable, inject } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IEmployeeDirectoryWithPagecount } from '../interface/iemployee-directory-with-pagecount';
import { EmployeeService } from './employee.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})

export class WolfDenService {

  emp = inject(EmployeeService);
  toastr=inject(ToastrService);
  router=inject(Router)

  private baseUrl = environment.apiUrl;
  public userId: number = 5;
  public role : string = "";
  public firstName: string = ""; 


  constructor(private http: HttpClient, private employeeService: EmployeeService) {
    if (localStorage.getItem('token') !== null) {
      const payload = this.emp.decodeToken();
      if(payload){
      this.userId = parseInt(payload.EmployeeId || 0, 10);
      this.role = (payload.Role||"");
      this.firstName = (payload.FirstName || 'welcome back');
      }else {
        this.toastr.error('Invalid or expired token', 'Error');
        this.router.navigate(['/user/login']); 
      }
    }
    else{
      this.toastr.error('No token found', 'Error');
      this.router.navigate(['/user/login']); 
    }
  }
  checkExpiry() {
    const payload = this.emp.decodeToken();
    if (payload) {
      const expTime = payload.exp; 
      const now = Date.now() / 1000; 
      const diff = expTime - (Math.floor(now));

      if (diff <= 0) {
        localStorage.removeItem('token');
        this.router.navigate(['/user/login']);
        this.toastr.error('Please login again', 'Session Timeout')
       
      }
    }
  }



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
  isLoggedIn(): boolean {
    return this.userId !== 0;
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
