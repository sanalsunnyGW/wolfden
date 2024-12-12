import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { IEmployeeDirectoryDto } from '../../../../interface/iemployee-directory';
import { WolfDenService } from '../../../../service/wolf-den.service';
import {MatPaginator, MatPaginatorModule, PageEvent} from '@angular/material/paginator';
import { IEmployeeDirectoryWithPagecount } from '../../../../interface/iemployee-directory-with-pagecount';
import { Router } from '@angular/router';
import { IDepartmentForm } from '../../../../interface/idepartment-form';
import { EmployeeService } from '../../../../service/employee.service';
import { IDepartment } from '../../../../interface/idepartment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-directory',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './employee-directory.component.html',
  styleUrl: './employee-directory.component.scss'
})
export class EmployeeDirectoryComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  employeesPagecount!: IEmployeeDirectoryWithPagecount;
  employees: IEmployeeDirectoryDto[] = [];
  departments: { id: number, name: string }[] = [];
  selectedDepartment: number | null = 0;
  searchTerm: string = '';
  private searchSubject = new Subject<string>();
  isLoading: boolean = false; 
  pageNumber: number = 0;
  pageSize: number = 5; 
  pageSizeOptions: number[] = [1, 5, 10, 20]; 
  totalRecords: number = 0;
  departmentData: IDepartment[] = [{
    id: 0,
    departmentName: ''
  }];

  constructor(private wolfDenService: WolfDenService,
              private router: Router, 
              private employeeService: EmployeeService, 
              private toastr: ToastrService) {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.loadEmployees();
    });
  }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartment();
  }
  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;
    console.log(this.pageNumber);
    this.pageSize = event.pageSize;
    this.loadEmployees();
  }
  routeToProfile(employeeId: number): void {
    this.router.navigate(['/portal/employee-display'], { queryParams: { id: employeeId } });
  }
  onSearch(): void {
    this.pageNumber = 0; 
    if (this.paginator) {
      this.paginator.firstPage(); 
    }
    this.loadEmployees();
  }
  loadDepartment(){
    this.employeeService.getAllDepartment().subscribe({
      next: (response: any) => {
        if (response) {
          this.departmentData = response;
        }
      },
      error: (error) => {
        this.toastr.error('An error occurred while Displaying Departments')

      }
    })
  }

  onDepartmentChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null;
    if (target) {
      const value = target.value;
      this.selectedDepartment = value ? Number(value) : 0;
      this.pageNumber = 0;
      if (this.paginator) {
        this.paginator.firstPage();
      }
      this.loadEmployees();
    }
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.wolfDenService.getAllEmployees(
      this.pageNumber,
      this.pageSize,
      this.selectedDepartment || undefined,
      this.searchTerm || undefined
    ).subscribe({
      next: (data) => {
        this.isLoading = false;
        this.employeesPagecount = data;
        this.totalRecords = data.totalPages;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.isLoading = false;
      }
    });
  }
}
