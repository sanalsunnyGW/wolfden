import { CommonModule, formatDate } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { getISOWeek, getYear } from 'date-fns';
import { Chart,registerables } from 'chart.js';
import { AttendanceService } from '../../../../../service/attendance.service';
import { WeeklyAttendance } from '../../../../../Interface/iweekly-attendance';

Chart.register(...registerables);

@Component({
  selector: 'app-weekly-attendance',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './weekly-attendance.component.html',
  styleUrl: './weekly-attendance.component.scss'
})
export class WeeklyAttendanceComponent {
  constructor() {}
  createChart() {
    {
      if (this.barChart) {
        this.barChart.destroy();
      }
      this.barChart = new Chart("canvas", {
        type: 'bar',
        data: {
          labels: [],  
          datasets: [{
            label: 'Weekly Attendance',
            data: [],  
            backgroundColor: [],  
            borderColor: [],  
            borderWidth: 1
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: {
              display: true, 
              position: 'top', 
              labels: {
                generateLabels: () => {
                  return [
                    {
                      text: 'Present',
                      fillStyle: '#72BF78', 
                    },
                    {
                      text: 'Absent',
                      fillStyle: '#AE445A', 
                    },
                    {
                      text: 'Incomplete Shift',
                      fillStyle: '#FCF596', 
                    },
                    {
                      text: 'Holiday',
                      fillStyle: '#AB886D', 
                    },
                    {
                      text: 'Work From Home',
                      fillStyle: '#536493', 
                    },
                    {
                      text:'Leave',
                      fillStyle:'#9B7EBD'
                    }
                  ];
                },
                usePointStyle: true, 
              }
            },
            tooltip: {
              callbacks: {
                label: (context: any) => {
                  return `${context.label}: ${context.raw} hours`;
                }
              }
            }
          },
          scales: {
            y: {
              beginAtZero: true
            }
          }
        }
      }); 
    }
  }
 service=inject(AttendanceService)
 selectedWeek!:string;
 offset=0;
 employeeId=1;
 weeklyData:WeeklyAttendance[]=[]
 barChart!:Chart;
 status:number[]=[]
 statusColor=["#72BF78","#AE445A","#FCF596","#AB886D","#536493","#9B7EBD"]
 ngOnInit(){
  const today=new Date();
  const year = getYear(today);
  const weekNumber = getISOWeek(today);
  this.selectedWeek=`${weekNumber},${year}`;
 }
getStartOfWeek(selectedWeek:string){
  if (selectedWeek) {
    this.createChart();
    const year = parseInt(this.selectedWeek.split('-W')[0], 10);
    const week = parseInt(this.selectedWeek.split('-W')[1], 10);
    const startDate = new Date(year, 0, 1);
    const dayOfWeek = startDate.getDay();
    let offset = 1 - dayOfWeek;  
    if (dayOfWeek === 0) { 
      offset = 1;
    }
    startDate.setDate(startDate.getDate() + offset);
    startDate.setDate(startDate.getDate() + (week - 1) * 7+1);
    const endDate = new Date(startDate);
    endDate.setDate(endDate.getDate() + 4);
    const formattedStartDate = startDate.toISOString().split('T')[0]; 
    const formattedEndDate = endDate.toISOString().split('T')[0];
    this.service.getWeeklyChart(this.employeeId,formattedStartDate,formattedEndDate).subscribe(
      (response: WeeklyAttendance[]) =>{
        if(response){
          this.weeklyData = response.map((item) => {
            const convertedDate = new Date(item.date);
            return { ...item, date: convertedDate };
          });
          this.barChart.data.labels=this.weeklyData.map((x:WeeklyAttendance)=>x.date.toLocaleDateString())
          this.barChart.data.datasets[0].data=this.weeklyData.map((x:WeeklyAttendance)=>x.insideDuration)
          this.status=this.weeklyData.map(x=>x.attendanceStatusId)
          this.barChart.data.datasets[0].backgroundColor,this.barChart.data.datasets[0].borderColor= this.weeklyData.map(x=>{
            if(x.attendanceStatusId===1)
            {
              return this.statusColor[0];
            }
            else if(x.attendanceStatusId===2)
            {
              return this.statusColor[1];
            }
            else if(x.attendanceStatusId===3)
            {
              return this.statusColor[2];
            }
            else if((x.attendanceStatusId===4)||(x.attendanceStatusId===5))
            {
              return this.statusColor[3];
            }
            else if((x.attendanceStatusId===6))
              {
                return this.statusColor[4];
              }
            else
            {
              return this.statusColor[5];
            }
          })
          this.barChart.update();
       }
        else {alert('Error fetching attendance:') }    
    });
  } 
}
}

