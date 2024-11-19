import { Routes } from '@angular/router';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';

import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';

import { UserComponent } from './user/user.component';

export const routes: Routes = [
    {
        path:'attendance/calendar',
        component:CalendarViewComponent
    },
    {
        path:'',
        component:UserComponent

    },
    {path:'leave-dashboard',component:LeaveDashboardComponent},
  {path:'leave-request-history',component:LeaveHistoryComponent}
    
   
];


