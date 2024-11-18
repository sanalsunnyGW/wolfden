import { CommonModule, formatDate } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AttendanceService } from '../../../../service/attendance.service';
import { Chart,registerables } from 'chart.js';
import { WeeklyAttendance } from '../../../../interface/iweekly-attendance';
Chart.register(...registerables);

@Component({
  selector: 'app-weekly-attendance',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './weekly-attendance.component.html',
  styleUrl: './weekly-attendance.component.scss'
})
export class WeeklyAttendanceComponent {
 service=inject(AttendanceService)

 selectedWeek: any;
 offset:number=0;
 employeeId:number=1;
 weeklyData:WeeklyAttendance[]=[]
 barChart!:Chart;
 status:string[]=[]

 statusColor={
  "present":"green",
  "absent":"red",
  "late":"yellow",
  "holiday":"brown",
  "wfh":"blue"
 }


getStartOfWeek(){
  if (this.selectedWeek) {
    const year = parseInt(this.selectedWeek.split('-W')[0], 10);
    const week = parseInt(this.selectedWeek.split('-W')[1], 10);
    const startDate = new Date(year, 0, 1);
    const dayOfWeek = startDate.getDay();
    let offset = 1 - dayOfWeek;  
    if (dayOfWeek === 0) { 
      offset = 1;
    }
    startDate.setDate(startDate.getDate() + offset);
    startDate.setDate(startDate.getDate() + (week - 1) * 7);
    const endDate = new Date(startDate);
    endDate.setDate(endDate.getDate()+4)
    const newStartDate=formatDate(startDate, 'yyyy-MM-dd', 'en-US');
    const newEndDate=formatDate(endDate, 'yyyy-MM-dd', 'en-US');
    this.service.getWeeklyChart(this.employeeId,newStartDate,newEndDate).subscribe(
      (response: WeeklyAttendance) =>{
        if(response){
          console.log(response)
          this.weeklyData.push(response)
      }
        else {console.error('Error fetching attendance:') }     
  });
  // this.barChart.data.labels=this.weeklyData.map((x:WeeklyAttendance)=>x.date)
  //this.barChart.data.datasets[0].data=this.weeklyData.map((x:WeeklyAttendance)=>x.duration)
  //this.status=this.weeklyData.map(x=>x.status)
  
  this.barChart.data.datasets[0].backgroundColor= this.weeklyData.map(x=>{
    if(x.status=="present")
    {
      const index=0;
    }
    else if(x.status=="present")

  })
  

  this.barChart =new Chart("canvas",  {
    type: 'bar',  
    data: {
      labels: ['a','b'],  
      datasets: [{
        label: 'Favourite Count',
        data: [2,4],  
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)',
          'rgba(54, 162, 235, 0.2)',
          'rgba(255, 206, 86, 0.2)',
          'rgba(75, 192, 192, 0.2)',
          'rgba(153, 102, 255, 0.2)'
        ],
        borderColor: [
          'rgba(255, 99, 132, 1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)',
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)'
        ],
        borderWidth: 1
      }]
    },
    options: {
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });
}
}
