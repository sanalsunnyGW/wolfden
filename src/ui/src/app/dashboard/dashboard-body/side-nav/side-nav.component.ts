import { Component, DestroyRef, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WolfDenService } from '../../../service/wolf-den.service';
import { EmployeeService } from '../../../service/employee.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { IEmployeeData } from '../../../interface/employee-data';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss',
  animations: [
    trigger('expandCollapse', [
      state('collapsed', style({
        height: '0',
        overflow: 'hidden'
      })),
      state('expanded', style({
        height: '*'
      })),
      transition('collapsed <=> expanded', [
        animate('200ms ease-in-out')
      ])
    ])
  ]
})
export class SideNavComponent {
  userService = inject(WolfDenService);
  employeeService = inject(EmployeeService);
  destroyRef = inject(DestroyRef);
  employeeHierarchyList: IEmployeeData[] = [{
    id: 0,
    employeeCode: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: new Date(),
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

  expandedSections: { [key: string]: boolean } = {
    leave: true,
    attendance: true
  };

  toggleSection(section: string): void {
    this.expandedSections[section] = !this.expandedSections[section];
  }

  isSectionExpanded(section: string): boolean {
    return this.expandedSections[section];
  }

  ngOnInit() {
    this.userService;
    this.employeeService.getMyTeamHierarchy(true, this.userService.userId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((data: any) => {
        this.employeeHierarchyList = data;
      });
  }

}
