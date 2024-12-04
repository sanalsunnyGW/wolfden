import { Component, inject } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CommonModule, formatDate } from '@angular/common';
import { AttendanceService } from '../../../../../service/attendance.service';
import { DailyAttendance } from '../../../../../interface/idaily-attendance';
import { WolfDenService } from '../../../../../service/wolf-den.service';
import { SplitCommaPipe } from "../../../../../pipe/split-comma.pipe";

@Component({
  selector: 'app-daily-attendence',
  standalone: true,
  imports: [CommonModule, SplitCommaPipe],
  templateUrl: './daily-attendence.component.html',
  styleUrl: './daily-attendence.component.scss'
})

export class DailyAttendenceComponent {
  service=inject(AttendanceService)
  baseService=inject(WolfDenService)
  constructor(private router: Router,private route:ActivatedRoute) {}
  attendanceDate!:string
  dailyData!:DailyAttendance
  present=false;
  notPresent=false;
  ngOnInit() {
   this.attendanceDate = this.route.snapshot.paramMap.get('attendanceDate')!;
   this.getDailyAttendence();
  }
 
  attendanceStatus = [
    { id: 1, viewValue: 'Present' },
    { id: 2, viewValue: 'Absent' },
    { id: 3, viewValue: 'Incomplete Shift' },
    { id: 4, viewValue: 'Restricted Holiday' },
    { id: 5, viewValue: 'Normal Holiday' },
    { id: 6, viewValue: 'WFH' },
    { id: 7, viewValue: 'Leave' },
    { id: 8, viewValue: 'OnGoing Shift' },
    { id: 9, viewValue: 'Half Day Leave'},
    {id : 10, viewValue: 'Weekend' }
  ];

  getAttendanceStatus(id:number)
  {
    const status = this.attendanceStatus.find(status => status.id === id);
    if(status?.id==4 || status?.id==5)
      status.viewValue="Holiday";
    return status ? status.viewValue : 'Unknown';
  }
  
  getDailyAttendence()
  {
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
  isMissed(missPunch:string)
  {
    if(missPunch)
    {
      this.present=true
    }
    else{
      this.notPresent=true;
    }
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
