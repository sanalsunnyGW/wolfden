import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { IEmployeeDirectoryDto } from '../../../../Interface/iemployee-directory';
import { WolfDenService } from '../../../../Service/wolf-den.service';
import {MatPaginatorModule, PageEvent} from '@angular/material/paginator';
import { IEmployeeDirectoryWithPagecount } from '../../../../Interface/iemployee-directory-with-pagecount';


@Component({
  selector: 'app-employee-directory',
  standalone: true,
  imports: [CommonModule, FormsModule,MatPaginatorModule],
  templateUrl: './employee-directory.component.html',
  styleUrl: './employee-directory.component.scss'
})
export class EmployeeDirectoryComponent implements OnInit {
  employeesPagecount!: IEmployeeDirectoryWithPagecount ;

  employees: IEmployeeDirectoryDto[] = [];
  departments: { id: number, name: string }[] = [];
  selectedDepartment: number | null = 0;
  searchTerm: string = '';
  private searchSubject = new Subject<string>();
  isLoading: boolean = false;
  pageNumber: number = 0;  
  pageSize: number = 1;   

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

  onPaginateChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;  
    this.pageSize = event.pageSize;
    this.loadEmployees();
  }
  onSearch(): void {
    this.loadEmployees(); 
    this.pageNumber = 0;
  }
  onDepartmentChange(event: Event): void {
    const target = event.target as HTMLSelectElement | null; 
    if (target) {
      const value = target.value;
      this.selectedDepartment = value ? Number(value) : 0; 
      this.pageNumber=0;
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
        console.log(data)
        this.isLoading = false; 
        this.employeesPagecount=data      
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.isLoading = false;
      }
    });
  }
}