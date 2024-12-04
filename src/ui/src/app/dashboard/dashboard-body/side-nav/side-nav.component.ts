import { Component, DestroyRef, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { WolfDenService } from '../../../service/wolf-den.service';
import { EmployeeService } from '../../../service/employee.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IEmployeeData } from '../../../interface/employee-data';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss'
})
export class SideNavComponent {
  userService=inject(WolfDenService)
  employeeService=inject(EmployeeService)
  destroyRef= inject(DestroyRef); 
  employeeHierarchyList:IEmployeeData[]=[{
    id: 0,
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth:new Date() ,
    designationId: 0,
    designationName: '',
    departmentId: 0,
    departmentName: '',
    managerId: 0,
    managerName: '',
    isActive: true,
    address: '',
    country: '',
    state: '',
    employmentType: 0,
    photo: '',
    subordinates: []
  }];


  ngOnInit()
{
  this.employeeService.getMyTeamHierarchy(true,this.userService.userId)
  .pipe(takeUntilDestroyed(this.destroyRef))
  .subscribe((data:IEmployeeData[])=> {
      this.employeeHierarchyList= data; 
  });
}
}
