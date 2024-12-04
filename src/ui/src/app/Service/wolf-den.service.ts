import { EmployeeService } from './employee.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { INotificationForm } from '../interface/i-notification-form';
import { ItodaysAbsence } from '../interface/itodays-absense';
import { IEmployeeDirectoryWithPagecount } from '../interface/iemployee-directory-with-pagecount';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../enviornments/environment';
import { Iholiday } from '../interface/iholiday';
@Injectable({
  providedIn: 'root'
})

export class WolfDenService {

  emp = inject(EmployeeService);
  toastr=inject(ToastrService);
  router=inject(Router)

  private baseUrl = environment.apiUrl;
  public userId: number = 0;
  public role : string = "";
  public firstName: string = ""; 


  constructor(private http: HttpClient, private employeeService: EmployeeService) {
    if (localStorage.getItem('token') !== null) {
      const payload = this.emp.decodeToken();
      if(payload){
      this.userId = parseInt(payload.EmployeeId || 0, 10);
      this.role = (payload.role||"");
      console.log(this.role)
      this.firstName = (payload.FirstName || 'welcome back');
      }else {
        this.toastr.error('Invalid or expired token', 'Error');
        this.router.navigate(['/user/login']); 
      }
    }
    else{
      this.toastr.error('login');
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
      `${this.baseUrl}/api/employee/sign-up`,
      { headers: this.getHeaders(), params }
    );
  }

  getEmployeeLogin(email: string, password: string): Observable<any> {
    const params = this.createHttpParams({
      Email: email,
      Password: password,
    });
    return this.http.get<any>(
      `${this.baseUrl}/api/employee/login`,
      { headers: this.getHeaders(), params }

    );
  }
  isLoggedIn(): boolean {
    return this.userId !== 0;
  }

  signIn(data: any): Observable<boolean> {
    return this.http.put<boolean>(`${this.baseUrl}/api/employee/employee-update-employee`, data, { headers: this.getHeaders() });
  }

  getAllEmployees(pageNumber: number, pageSize: number, departmentId?: number, employeeName?: string): Observable<IEmployeeDirectoryWithPagecount> {
    const params = this.createHttpParams({
      PageNumber: pageNumber,
      PageSize: pageSize,
      DepartmentID: departmentId,
      EmployeeName: employeeName,
    });
    return this.http.get<IEmployeeDirectoryWithPagecount>(
      `${this.baseUrl}/api/employee/all`,
      { headers: this.getHeaders(), params }
    );
  }
//notification
  getNotification(employeeId: number): Observable<INotificationForm[]> {
    const params = this.createHttpParams({
      EmployeeId:employeeId
    });
    return this.http.get<INotificationForm[]>(
      `${this.baseUrl}/api/Notification/employee`,
      { headers: this.getHeaders(), params }
    );
  }
  getHoliday(): Observable<Iholiday[]>{
    const params=this.createHttpParams({});
    return this.http.get<Iholiday[]>(
      `${this.baseUrl}/api/Holiday/upcoming-holiday`,
      {headers: this.getHeaders(), params }
    );
  }


  getTodaysAbsence():Observable<ItodaysAbsence[]>{
    const params= this.createHttpParams({});
    return this.http.get<ItodaysAbsence[]>(
      `${this.baseUrl}/api/LeaveRequestDay/leaves-on-current-day`,
      { headers: this.getHeaders(), params }
    );
  }
  markAsRead(notificationId: number): Observable<boolean> {
    return this.http.patch<boolean>(`${this.baseUrl}/api/Notification/read`, {notificationId}, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
  checkPassword(id: number, password: string): Observable<boolean> {
    const params = this.createHttpParams({
      Id: id,
      Password: password,
    });
    return this.http.get<boolean>(
      `${this.baseUrl}/api/employee/check-password`,
      { headers: this.getHeaders(), params }

    );
  }

  resetPassword(id: number, password:string): Observable<boolean> {
    const url = `${this.baseUrl}/api/employee/reset-password`;
    const payload = { id,password }; 
    return this.http.patch<boolean>(url, payload, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }
}