// employee-directory.component.ts
import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { WolfDenService } from '../../../../service/wolf-den.service';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { IEmployeeDirectoryWithPagecount } from '../../../../interface/iemployee-directory-with-pagecount';
import { IEmployeeDirectoryDto } from '../../../../interface/iemployee-directory';

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
  pageSize: number = 1; 
  pageSizeOptions: number[] = [1, 2, 3, 4];
  totalRecords: number = 0;

  constructor(private wolfDenService: WolfDenService) {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.loadEmployees();
    });
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;
    console.log(this.pageNumber);
    this.pageSize = event.pageSize;
    this.loadEmployees();
  }

  onSearch(): void {
    this.pageNumber = 0; 
    if (this.paginator) {
      this.paginator.firstPage(); 
    }
    this.loadEmployees();
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
        this.totalRecords = data.totalRecords;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.isLoading = false;
      }
    });
  }
}