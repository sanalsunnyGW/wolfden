import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../enviornments/environment';

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
    return this.http.post(`${this.baseUrl}/api/Employee/EmployeeUpdateEmployee`, data, { headers: this.getHeaders() });
  }
}
