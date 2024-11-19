import { Component, OnInit } from '@angular/core';
import { Employee } from './iemployee-hierarchy';
import { EmployeeSubordinatesComponent } from './employee-subordinates/employee-subordinates.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employee-hierarchy',
  standalone: true,
  imports: [CommonModule,EmployeeSubordinatesComponent],
  templateUrl: './employee-hierarchy.component.html',
  styleUrl: './employee-hierarchy.component.scss'
})
export class EmployeeHierarchyComponent implements OnInit{
  employeeHierarchy: Employee | null = null; 

  ngOnInit(): void {
    this.employeeHierarchy = {
      id: 1,
      employeeCode: 777,
      firstName: 'Abhi',
      lastName: 'Kumar',
      email: 'abhi.kumar@example.com',
      phoneNumber: '1234567890',
      dateofBirth: new Date('1990-05-15'),
      designationId: 2,
      departmentId: 1,
      managerId: null,
      isActive: true,
      subordinates: [
        {
          id: 2,
          employeeCode: 900,
          firstName: 'Nohan',
          lastName: 'Antony',
          email: 'nohanantony@gmail.com',
          phoneNumber: '892127666',
          dateofBirth: new Date('1995-11-20'),
          designationId: 1,
          departmentId: 1,
          managerId: 1,
          isActive: true,
          subordinates: [
            {
              id: 3,
              employeeCode: 901,
              firstName: 'Ravi',
              lastName: 'Sharma',
              email: 'ravi.sharma@example.com',
              phoneNumber: '9888776655',
              dateofBirth: new Date('1992-08-12'),
              designationId: 3,
              departmentId: 2,
              managerId: 2,
              isActive: true,
              subordinates: [] 
            },
            {
              id: 4,
              employeeCode: 902,
              firstName: 'Saurabh',
              lastName: 'Verma',
              email: 'saurabh.verma@example.com',
              phoneNumber: '9988774455',
              dateofBirth: new Date('1994-01-25'),
              designationId: 4,
              departmentId: 3,
              managerId: 2,
              isActive: true,
              subordinates: [{
                id: 7,
                employeeCode: 905,
                firstName: 'Kartik',
                lastName: 'Yadav',
                email: 'kartik.yadav@example.com',
                phoneNumber: '9998776655',
                dateofBirth: new Date('1999-05-20'),
                designationId: 7,
                departmentId: 5,
                managerId: 6,
                isActive: true,
                subordinates: [] // No further subordinates
              }] // No subordinates for Saurabh Verma
            },
            {
              id: 5,
              employeeCode: 903,
              firstName: 'Shubham',
              lastName: 'Singh',
              email: 'shubham.singh@example.com',
              phoneNumber: '9008779988',
              dateofBirth: new Date('1996-07-18'),
              designationId: 5,
              departmentId: 3,
              managerId: 2,
              isActive: true,
              subordinates: [] // No subordinates for Shubham Singh
            }
          ]
        }
      ]
    };
  }
}



