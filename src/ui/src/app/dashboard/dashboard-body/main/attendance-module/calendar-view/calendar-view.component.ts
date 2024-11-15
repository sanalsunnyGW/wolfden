import { Component } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core/index.js';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';

@Component({
  selector: 'app-calendar-view',
  standalone: true,
  imports: [FullCalendarModule],
  templateUrl: './calendar-view.component.html',
  styleUrl: './calendar-view.component.scss'
})
export class CalendarViewComponent {

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: (arg) => this.handleDateClick(arg),
    dayCellClassNames: (arg) => this.getWeekendClassNames(arg),
    
        height: 500, // Automatically adjust height based on content
       // contentHeight: 'auto', // Adjust content height to fit the calendar without scrollbars
       // aspectRatio: 1.5, // Controls the aspect ratio of the calendar, try tweaking to get a better fit
        
        // slotDuration: '00:20:00', // Adjust slot size for smaller time increments (15 mins)
        dayHeaderFormat: { weekday: 'short' }, //
    // events: [
    //   { title: 'event 1', date: '2024-11-01' },
    //   { title: 'event 2', date: '2019-04-02' }
    // ]
  };

  handleDateClick(arg: DateClickArg) {
    alert('date click! ' + arg.dateStr)
  }

  getWeekendClassNames(arg: any): string[] {
    const isSaturday = arg.date.getDay() === 6;  // 6 is Saturday
    const isSunday = arg.date.getDay() === 0;  // 0 is Sunday

    if (isSaturday || isSunday) {
      return ['weekend-day'];  // Apply custom class for weekends
    }

    return [];
  }
  
}
