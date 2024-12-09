// main-page.component.ts
import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveManagementService } from '../../../../service/leave-management.service';
import { WolfDenService } from '../../../../service/wolf-den.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILeaveBalanceList } from '../../../../interface/leave-balance-list-interface';
import { WeeklyAttendanceComponent } from "../attendance-module/weekly-attendance/weekly-attendance.component";
import { ItodaysAbsence } from '../../../../interface/itodays-absense';
import { Iholiday } from '../../../../interface/iholiday';


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
  imports: [CommonModule, WeeklyAttendanceComponent],
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})
export class MainPageComponent implements OnInit {
 //leave balance 
  leaveManagementService = inject(LeaveManagementService)
  userService = inject(WolfDenService)
  destroyRef = inject(DestroyRef)
  leaveList: ILeaveBalanceList[] = [];
  todaysAbsences: ItodaysAbsence[]=[];
  holidayList:Iholiday[]=[];
  leaveType1: string = '';
  leaveBalance1: number = 0;
  leaveType2: string = '';
  leaveBalance2: number = 0;
  leaveType3: string = '';
  leaveBalance3: number = 0;
  ngOnInit() {
    //todays absence
    this.userService.getTodaysAbsence().subscribe((data)=>{
      this.todaysAbsences=data;
    })

    this.userService.getHoliday().subscribe((data)=>{
      this.holidayList=data;
    })

    this.leaveManagementService.getLeaveBalance(this.userService.userId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((data) => {
        this.leaveList = data;
        if (this.leaveList.length > 2) {
          // Leave type 1        
            this.leaveType1 = this.leaveList[0].name;
            this.leaveBalance1 = this.leaveList[0].balance;
          
  
          // Leave type 2         
            this.leaveType2 = this.leaveList[1].name;
            this.leaveBalance2 = this.leaveList[1].balance;
          
  
          // Leave type 3          
            this.leaveType3 = this.leaveList[2].name;
            this.leaveBalance3 = this.leaveList[2].balance;
          
        }
      });
  }
}