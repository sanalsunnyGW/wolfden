// main-page.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';


interface Absence {
  name: string;
  department: string;
  reason: string;
}

interface Holiday {
  date: string;
  name: string;
}

interface CalendarDay {
  date: number | '';
  isHoliday: boolean;
  holidayName?: string;
}

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {
  leavesRemaining = 10;
  hoursRemaining = 6.5;
  currentDate = new Date();
  weekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  calendarDays: CalendarDay[] = [];
  day= this.calendarDays;

  todaysAbsences: Absence[] = [
    { name: 'Eldo P Joy', department: 'Developer', reason: 'Sick Leave' },
    { name: 'Aravind', department: 'Developer', reason: 'Vacation' },
    { name: 'Siva Prakash', department: 'QA', reason: 'Personal Leave' },
    { name: 'Irfan', department: 'Developer', reason: 'Work from Home' },
    { name: 'Prem', department: 'Finance', reason: 'Sick Leave' }
  ];
   absence=this.todaysAbsences;

  holidays: Holiday[] = [
    { date: '2024-01-26', name: 'Republic Day' },
    { date: '2024-08-15', name: 'Independence Day' },
    { date: '2024-10-02', name: 'Gandhi Jayanti' },
    {date : '2024-11-15',name: 'National Holiday'},
  ];

  
  ngOnInit() {
    this.generateCalendar();
  }

  private generateCalendar() {
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();
    const firstDay = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    
    //empty cells for days before the first of the month
    for (let i = 0; i < firstDay; i++) {
      this.calendarDays.push({ date: '', isHoliday: false });
    }

    // Add actual days
    for (let day = 1; day <= daysInMonth; day++) {
      const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
      const holiday = this.holidays.find(h => h.date === dateStr);
      
      this.calendarDays.push({
        date: day,
        isHoliday: !!holiday,
        holidayName: holiday?.name
      });
    }
  }

  getMonthAndYear(): string {
    return this.currentDate.toLocaleString('default', { month: 'long', year: 'numeric' });
  }
}