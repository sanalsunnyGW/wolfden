import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule} from '@angular/common';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AttendanceService } from '../../../../../service/attendance.service';
import { ICheckAttencdanceClose } from '../../../../../interface/check-attendance-close';
import { allEmployeesMonthlyReports, MonthlyReports } from '../../../../../interface/monthly-report';
import { GetRange } from '../../../../../interface/get-range';
import { NgSelectModule } from '@ng-select/ng-select';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-monthly-report',
  standalone: true,
  imports: [FormsModule,CommonModule,MatPaginatorModule,NgSelectModule],
  templateUrl: './monthly-report.component.html',
  styleUrl: './monthly-report.component.scss'
})

export class MonthlyReportComponent {
  service=inject(AttendanceService)
  checkStatus=false
  selectedClosedDate:string=''
  selectedPreviousDate:string=''
  pageNumber=-1;
  pageSize=10;
  pageSizes=[5,10,30,50];
  totalPages=0;
  concatenatedDates: string[]=[];
  selectedRange!: string;
  monthlyData!:MonthlyReports
  employeeReport:allEmployeesMonthlyReports[]=[]
  previousClosedDate:string[]=[]
  attendanceClosedDate:string[]=[]
  today:Date=new Date()
  dateOnly: string = this.today.toISOString().split('T')[0];
  lastClosedDate:string=''
  isPreviewMode: boolean = false;

constructor(private toastr: ToastrService){

}
  ngOnInit() {
    this.getDateRange();
    this.getStatus();
    
  }
  
 
  getStatus()
  {
    this.checkStatus=false
    this.service.checkAttendanceClose(this.dateOnly).subscribe(
      (response: ICheckAttencdanceClose) =>{
        if(response.status==false)
        {
          this.checkStatus=true;
        }  
      });  
  }

  getMonthlyReport(pageNumber: number, pageSize: number) {
    this.isPreviewMode = false;
  
   
    if (this.selectedRange && this.selectedRange.includes('to')) {
    
      const [startDate, endDate] = this.selectedRange.split('to').map(s => s.trim());
      

      this.selectedPreviousDate = startDate;
      this.selectedClosedDate = endDate;

      console.log(this.selectedClosedDate, this.selectedPreviousDate);
  
      this.service.getMonthlyReport(this.selectedPreviousDate, this.selectedClosedDate, pageNumber, pageSize)
        .subscribe(
          (response: MonthlyReports) => {
            if (response) {
              console.log(response);
              this.monthlyData = response;
              this.totalPages = this.monthlyData.pageCount;
              this.employeeReport = this.monthlyData.allEmployeesMonthlyReports;
            } else {
              alert("No data found");
            }
          }
        );
    } else {

      console.error("Invalid selectedRange format. Please use 'startDate to endDate'.");
      alert("Please select a valid date range.");
    }
  }
  

  getPreviewReport(pageNumber:number,pageSize:number)
  {
    this.isPreviewMode = true;
    this.lastClosedDate = this.attendanceClosedDate[this.attendanceClosedDate.length - 1]; 
    this.service.getMonthlyReport(this.lastClosedDate,this.dateOnly,pageNumber,pageSize).subscribe(
      (response: MonthlyReports) =>{
        if(response){
          this.monthlyData=response
          this.totalPages=this.monthlyData.pageCount;
          this.employeeReport=this.monthlyData.allEmployeesMonthlyReports
      }
      else {alert("no data found") };
   });  
  } 
  getDateRange()
  {
    this.service.getRange().subscribe(
      (response:GetRange)=>{
        this.attendanceClosedDate=response.attendanceClosedDate;
        this.previousClosedDate=response.previousAttendanceClosedDate;
     this.concatenatedDates = [];
        for (let i = 0; i < this.attendanceClosedDate.length; i++) {
          this.concatenatedDates[i]=(`${this.previousClosedDate[i]} to ${this.attendanceClosedDate[i]}`);
        }
      }
    )


  }
  closeAttendance()
  {
    this.service.closeAttendance().subscribe(
      (response:any) =>{
        if(response){
         // alert("attendence Closed");
          this.checkStatus=false
       
          this.toastr.success("Attendance Closed")
          this.getDateRange();
        
      }
      else {this.toastr.error("no data found") };
   });  

   
  }

 
  onPaginateChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex;  
    this.pageSize = event.pageSize;
    if (this.isPreviewMode) {
      this.getPreviewReport(this.pageNumber, this.pageSize);
    } else {
      this.getMonthlyReport(this.pageNumber, this.pageSize);
    }
  }
}
