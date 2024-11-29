import { AfterViewInit, Component, inject, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular';
import { Calendar, CalendarOptions, DatesSetArg, DayCellContentArg } from '@fullcalendar/core/index.js';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';
import { AttendanceService } from '../../../../../service/attendance.service';
import { IAttendanceSummary } from '../../../../../interface/attendance-summary';
import { IAttendanceData } from '../../../../../interface/attendance-data';
import { Router } from '@angular/router';
import { WolfDenService } from '../../../../../service/wolf-den.service';

@Component({
  selector: 'app-calendar-view',
  standalone: true,
  imports: [FullCalendarModule],
  templateUrl: './calendar-view.component.html',
  styleUrl: './calendar-view.component.scss'
})
export class CalendarViewComponent implements OnInit  {

  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent;

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

  currentYear: number = new Date().getFullYear();
  currentMonth: number = new Date().getMonth() + 1;
  attendanceData: { [date: string]: number } = {};

  constructor(private router:Router) {
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
      this.calendarComponent.getApi().render(); 
    });
  }

  newDate!:string
  handleDateClick(arg: DateClickArg) {
    const selectedDate = arg.dateStr;
    this.newDate=selectedDate;
    this.router.navigate(['portal/attendance/daily', this.newDate]);
  }
  getDayCellClassNames(arg: DayCellContentArg): string[] {
    const date = new Date(Date.UTC(
      arg.date.getFullYear(),
      arg.date.getMonth(),
      arg.date.getDate()
    ));
    const currentDate = arg.view.currentStart; 
    const currentYear = currentDate.getFullYear();
    const currentMonth = currentDate.getMonth(); 

    if (date.getFullYear() === currentYear && date.getMonth() === currentMonth) {
      const dateStr = date.toISOString().split('T')[0];
      const status = this.attendanceData[dateStr];

      if (arg.date.getDay() === 6 || arg.date.getDay() === 0) {
        return ['weekend-day'];
      }
      
      switch(status)
      {
        case 1:return ['present'];
        case 2:return ['absent'];
        case 3:return ['incompleteShift'];
        case 4:return ['wfh'];
      }
    }

    return [];
  }

  onCalendarMonthChange(arg: DatesSetArg): void {
    const currentDate = arg.view.currentStart;
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth() + 1;
    this.getStatusData(year, month);
    this.fetchAttendanceData(year, month);
  }
}
