import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { NgSelectModule } from '@ng-select/ng-select';
import { AttendanceService } from '../../../../../service/attendance.service';
import { AttendanceHistory } from '../../../../../interface/attendance-history';
import { WolfDenService } from '../../../../../service/wolf-den.service';

@Component({
  selector: 'app-attendance-history',
  standalone: true,
  imports: [ReactiveFormsModule,NgSelectModule,CommonModule,FormsModule,MatPaginatorModule],
  templateUrl: './attendance-history.component.html',
  styleUrl: './attendance-history.component.scss'
})
export class AttendanceHistoryComponent implements OnInit {

  service=inject(AttendanceService);
  baseService=inject(WolfDenService);
  selectedYear: number;
  selectedMonth!: number;
  selectedStatus: number=0;  
  selectedPageSize: number = 5;   
  selectedPageNumber:number=0;
  attendanceData: any[] = [];
 
  employeeId=this.baseService.userId;
  years: number[] = [];
  pageSizes = [5, 10, 20, 30, 40];
  totalPages!: number;

constructor() 
{
  this.selectedYear = new Date().getFullYear();
  for (let year = this.selectedYear; year >= 2020; year--) {
    this.years.push(year);
  }
}

ngOnInit(): void {
}

months = [
  { id: 1, name: 'January' },
  { id: 2, name: 'February' },
  { id: 3, name: "March" },
  { id: 4, name: 'April' },
  { id: 5, name: 'May' },
  { id: 6, name: "June" },
  { id: 7, name: 'July' },
  { id: 8, name: 'August' },
  { id: 9, name: "September" },
  { id: 10, name: 'October' },
  { id: 11, name: 'November' },
  { id: 12, name: "December" },
];

status = [
  { id: 1, name: 'Present' },
  { id: 2, name: 'Absent' },
  { id: 3, name: "Incomplete Shift" },
  { id: 4, name: 'Restricted Holiday' },
  { id: 5, name: 'Normal Holiday' },
  { id: 6, name: "WFH" },
  { id: 7, name: 'Leave' },
  { id: 9, name:'Half Day'}
];

fetchHistory(){
  this.service.getMonthlyHistoryByStatus(
    this.employeeId,
    this.selectedYear,
    this.selectedMonth,
    this.selectedStatus,
    this.selectedPageNumber,
    this.selectedPageSize).subscribe(
    (response:AttendanceHistory)=>{
      this.attendanceData=response.attendanceHistory;
      this.totalPages=response.totalPages;
    }
  )
}

getStatusName(statusId: number): string {
  const statusObj = this.status.find(s => s.id === statusId);
  return statusObj ? statusObj.name : 'Unknown';  
}

onPageChange(event:PageEvent):void{
  this.selectedPageNumber = event.pageIndex;
  this.selectedPageSize = event.pageSize;
  this.fetchHistory();  
}

onSubmit(): void {
  this.selectedPageNumber = 0;  
  this.fetchHistory();  
}
}
