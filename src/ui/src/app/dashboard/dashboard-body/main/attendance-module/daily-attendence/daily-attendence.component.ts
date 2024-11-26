import { Component, inject } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CommonModule, formatDate } from '@angular/common';
import { AttendanceService } from '../../../../../Service/attendance.service';
import { DailyAttendance } from '../../../../../Interface/idaily-attendance';
import { WolfDenService } from '../../../../../Service/wolf-den.service';


@Component({
  selector: 'app-daily-attendence',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './daily-attendence.component.html',
  styleUrl: './daily-attendence.component.scss'
})

export class DailyAttendenceComponent {
  service=inject(AttendanceService)
  baseService=inject(WolfDenService)
  constructor(private router: Router,private route:ActivatedRoute) {}
  attendanceDate!:string
  dailyData!:DailyAttendance
  ngOnInit() {
   this.getDailyAttendence();
  }
 
  attendanceStatus = [
    { id: 1, viewValue: 'Present' },
    { id: 2, viewValue: 'Absent' },
    { id: 3, viewValue: 'IncompleteShift' },
    { id: 4, viewValue: 'RestrictedHoliday' },
    { id: 5, viewValue: 'NormalHoliday' },
    { id: 6, viewValue: 'WFH' },
    { id: 7, viewValue: 'Leave' },
  ];

  getAttendanceStatus(id:number)
  {
    const status = this.attendanceStatus.find(status => status.id === id);
    return status ? status.viewValue : 'Unknown';
  }
  
  getDailyAttendence()
  {
    this.attendanceDate = this.route.snapshot.paramMap.get('attendanceDate')!;
    const employeeId=this.baseService.userId;
    const selectedDate=new Date(this.attendanceDate)
    const date=formatDate(selectedDate, 'yyyy-MM-dd', 'en-US');
    this.service.getDailyAttendence(employeeId,date).subscribe(
      (response: DailyAttendance) =>{
        if(response){
          this.dailyData=response;  
      }
        else {alert("no attendance found") };
   });   
  }
  minutess:string='';
  NumToTime(num: number): string { 
    const hours = Math.floor(num / 60);  
    let minutes = num % 60;
    if (minutes < 10) {
      this.minutess = '0' + minutes; 
    } 
    return `${hours}h ${minutes}m`;
  }
  convertToTime(dateStr: string): string {
    if(dateStr)
    {
    const date = new Date(dateStr); 
    const hours = date.getHours().toString().padStart(2, '0'); 
    const minutes = date.getMinutes().toString().padStart(2, '0'); 
    return `${hours}:${minutes}`; 
    }
    return ''
  }
  downloadDailyReport()
  {
    this.attendanceDate = this.route.snapshot.paramMap.get('attendanceDate')!;
    const employeeId=this.baseService.userId;
    const selectedDate=new Date(this.attendanceDate)
    const selected=formatDate(selectedDate, 'yyyy-MM-dd', 'en-US');
    this.service.downloadDailyReport(employeeId,selected).subscribe(
      (response: any) =>{
        let blob:Blob=response.body as Blob;
        let url=window.URL.createObjectURL(blob);
        window.open(url);      
  });   
  } 
}
