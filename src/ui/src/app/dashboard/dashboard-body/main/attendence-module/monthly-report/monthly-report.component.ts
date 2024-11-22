import { Component, inject } from '@angular/core';
import { AttendanceService } from '../../../../../service/attendance.service';
import { allEmployeesMonthlyReports, MonthlyReports } from '../../../../../interface/monthly-report';
import { FormsModule } from '@angular/forms';
import { CommonModule, formatDate } from '@angular/common';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-monthly-report',
  standalone: true,
  imports: [FormsModule,CommonModule,MatPaginatorModule],
  templateUrl: './monthly-report.component.html',
  styleUrl: './monthly-report.component.scss'
})
export class MonthlyReportComponent {
  service=inject(AttendanceService)
  selectedMonth: any;
  monthNumber:number=0
  yearNumber:number=0
  checkStatus:boolean=false
  checkClosedStatus:boolean=false
  monthIsSelected:boolean=false
  display:boolean=false
  pageNumber: number=1;
  pageSize:number=1
  totalPages: number=0;
  constructor() {}
  monthlyData!:MonthlyReports
  EmployeeReport:allEmployeesMonthlyReports[]=[]
  ngOnInit() {}
  monthNames: string[] = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];
  getMonth(selectedMonth:any)
  {
    this.display=false
    this.checkClosedStatus=false
    this.checkStatus=false
    const year = parseInt(selectedMonth.split('-')[0], 10);
    const month = parseInt(selectedMonth.split('-')[1], 10);
    this.monthNumber=Number(month);
    this.yearNumber=Number(year);
    this.service.checkAttendanceClose(this.monthNumber,this.yearNumber).subscribe(
      (response: boolean) =>{
        if(response==true){
          this.checkStatus=true;
      }
      else{
        this.checkClosedStatus=true;
      }
  });  
  }
  getMonthlyReport(selectedMonth:any,pageNumber:number,pageSize:number)
  {
    const year = parseInt(selectedMonth.split('-')[0], 10);
    const month = parseInt(selectedMonth.split('-')[1], 10);
    this.monthNumber=Number(month);
    this.yearNumber=Number(year);
    this.service.getMonthlyReport(this.monthNumber,this.yearNumber,pageNumber,pageSize).subscribe(
      (response: MonthlyReports) =>{
        if(response){
          this.monthlyData=response
          this.totalPages=this.monthlyData.pageCount;
          this.EmployeeReport=this.monthlyData.allEmployeesMonthlyReports
          this.display=true
      }
      else {alert("no data found") };
   });  
  } 
  closeAttendance(selectedMonth:any)
  {
    const year = parseInt(selectedMonth.split('-')[0], 10);
    const month = parseInt(selectedMonth.split('-')[1], 10);
    this.monthNumber=Number(month);
    this.yearNumber=Number(year);
    this.service.closeAttendance(this.monthNumber,this.yearNumber).subscribe(
      (response:any) =>{
        if(response){
          alert("attendence Closed");
          this.checkClosedStatus=false
          this.checkStatus=true
      }
      else {alert("no data found") };
   });  
  }
  onPaginateChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;  
    this.pageSize = event.pageSize;
    this.getMonthlyReport(this.selectedMonth,this.pageNumber,this.pageSize);
  }
}
