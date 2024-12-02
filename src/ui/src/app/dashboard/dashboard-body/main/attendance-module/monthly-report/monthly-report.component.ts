import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule, formatDate } from '@angular/common';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AttendanceService } from '../../../../../service/attendance.service';
import { allEmployeesMonthlyReports, MonthlyReports } from '../../../../../interface/monthly-report';
import { ICheckAttencdanceClose } from '../../../../../interface/check-attendance-close';

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
  monthNumber=0
  yearNumber=0
  checkStatus=false
  checkClosedStatus=false
  monthIsSelected=false
  display=false
  pageNumber=-1;
  pageSize=5;
  pageSizes=[5,10,15,20]
  totalPages=0;
  constructor() {}
  monthlyData!:MonthlyReports
  employeeReport:allEmployeesMonthlyReports[]=[]
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
    console.log(this.monthNumber)
    this.yearNumber=Number(year);
    this.service.checkAttendanceClose(this.monthNumber,this.yearNumber).subscribe(
      (response: ICheckAttencdanceClose) =>{
        if(response.status==true){
          this.checkStatus=true;
      }
      else{
        const currentDate=new Date()
        const month=currentDate.getMonth();
        console.log(month)
        if(month+1==this.monthNumber)
        this.checkClosedStatus=true;
        else
        alert(`Sorry,you cant close the attendance of ${this.monthNames[this.monthNumber-1]} on ${this.monthNames[month]}`)
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
          this.employeeReport=this.monthlyData.allEmployeesMonthlyReports
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
    this.pageNumber = event.pageIndex;  
    this.pageSize = event.pageSize;
    this.getMonthlyReport(this.selectedMonth,this.pageNumber,this.pageSize);
  }
}
