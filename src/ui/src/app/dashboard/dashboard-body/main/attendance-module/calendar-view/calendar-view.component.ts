import { Component, inject, OnInit } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions, DatesSetArg, DayCellContentArg } from '@fullcalendar/core/index.js';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';
import { AttendanceService } from '../../../../../Service/attendance.service';
import { IAttendanceSummary } from '../../../../../Interface/attendance-summary';
import { IAttendanceData } from '../../../../../Interface/attendance-data';
import { Router } from '@angular/router';
import { WolfDenService } from '../../../../../Service/wolf-den.service';



@Component({
  selector: 'app-calendar-view',
  standalone: true,
  imports: [FullCalendarModule],
  templateUrl: './calendar-view.component.html',
  styleUrl: './calendar-view.component.scss'
})
export class CalendarViewComponent implements OnInit {

  service = inject(AttendanceService);
  baseService=inject(WolfDenService)

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    height: 500,
    dayHeaderFormat: { weekday: 'short' },
    dateClick: (arg) => this.handleDateClick(arg),
    datesSet: (arg) => this.onCalendarMonthChange(arg),
    dayCellClassNames: (arg) => this.getDayCellClassNames(arg)
  };

  present: number = 0;
  absent: number = 0;
  incompleteShift: number = 0;
  wfh: number = 0;

  //employeeId: number = 1;
  currentYear: number = new Date().getFullYear();
  currentMonth: number = new Date().getMonth() + 1;
  attendanceData: { [date: string]: number } = {};
  

  constructor(private router:Router) {
    this.attendanceData = {};
  }

  ngOnInit(): void {
    this.fetchAttendanceData(this.currentYear, this.currentMonth);
    this.getStatusData(this.currentYear, this.currentMonth);
  }


  fetchAttendanceData(year: number, month: number): void {
    this.service.getAttendanceSummary(this.baseService.userId, year, month).subscribe((data: IAttendanceSummary) => {
      this.present = data.present;
      this.absent = data.absent;
      this.incompleteShift = data.incompleteShift;
      this.wfh = data.wfh;
    });
  }

  getStatusData(year: number, month: number) {
    this.service.getDailyStatus(this.baseService.userId, year, month).subscribe((data: IAttendanceData[]) => {
      data.forEach((item: IAttendanceData) => {
        this.attendanceData[item.date] = item.attendanceStatusId;
      });
    });
  }
 newDate!:string
  handleDateClick(arg: DateClickArg) {
   //alert('date click! ' + arg.dateStr)
    const selectedDate = arg.dateStr;
    this.newDate=selectedDate;
    // // Navigate to a new route, passing the selected date as a query parameter
    this.router.navigate(['dashboard/attendance/daily', this.newDate]);

  }

  getDayCellClassNames(arg: DayCellContentArg): string[] {
    const date = new Date(arg.date);
    date.setDate(date.getDate() + 1);
    const dateStr = date.toISOString().split('T')[0];
    const status = this.attendanceData[dateStr];

    if (arg.date.getDay() === 6 || arg.date.getDay() === 0) {
      return ['weekend-day'];
    }

    

    if (status === 1) {
      return ['present'];
    } else if (status === 2) {
      return ['absent'];
    } else if (status === 3) {
      return ['incompleteShift'];
    } else if (status === 4) {
      return ['wfh'];
    }
    return [];
  }

  onCalendarMonthChange(arg: DatesSetArg): void {
    const currentDate = arg.view.currentStart;
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth() + 1;
    this.fetchAttendanceData(year, month);
    this.getStatusData(year, month);
  }

}
