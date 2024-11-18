import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

import { CommonModule, formatDate } from '@angular/common';
import { DailyAttendance } from '../../../../../Interfaces/idaily-attendance';
import { AttendanceService } from '../../../../../Service/attendance.service';


@Component({
  selector: 'app-daily-attendence',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './daily-attendence.component.html',
  styleUrl: './daily-attendence.component.scss'
})
export class DailyAttendenceComponent {

  service=inject(AttendanceService)
  
  constructor(private router: Router) {
    
  }
  dailyData!:DailyAttendance

  ngOnInit() {
    const selectedDate=new Date('2024-11-11')
    const selected=formatDate(selectedDate, 'yyyy-MM-dd', 'en-US');
    this.getDailyAttendence(selected)

  }

  getDailyAttendence(date:string)
  {
    //const employeeId=localStorage.getItem('employeeId');
    const employeeId=1;
    this.service.getDailyAttendence(employeeId,date).subscribe(
      (response: DailyAttendance) =>{
        if(response){
          console.log(response)
          this.dailyData=response;
          
      }
        else {console.error('Error fetching attendance:') }     
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
    const date = new Date(dateStr); 
    const hours = date.getHours().toString().padStart(2, '0'); 
    const minutes = date.getMinutes().toString().padStart(2, '0'); 
    return `${hours}:${minutes}`; 
  }
  
  downloadDailyReport()
  {
    //const employeeId=localStorage.getItem('employeeId');
    const employeeId=1;
    const selectedDate=new Date('2024-11-11')
    const selected=formatDate(selectedDate, 'yyyy-MM-dd', 'en-US');
    this.service.downloadDailyReport(employeeId,selected).subscribe(
      (response: any) =>{
        let blob:Blob=response.body as Blob;
        let url=window.URL.createObjectURL(blob);
        window.open(url);      
  });   
  }
  
}
