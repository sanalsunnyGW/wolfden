import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {

  constructor() { }
  private http=inject(HttpClient);


  getHierarchy()
  {
    return this.http.get("https://localhost:7015/api/Employee/hierarchy");
  }
  getEmployeeProfile()
  {
    return this.http.get("https://localhost:7015/api/Employee/by-Id?EmployeeId=1");
  }
  employeeUpdateEmployee(userForm:any)
  {
    return this.http.put("https://localhost:7015/api/Employee/employee-update-employee",userForm)
  }
}