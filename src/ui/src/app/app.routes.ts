import { Routes } from '@angular/router';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';

import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { EditLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/edit-leave-type/edit-leave-type.component';
import { UpdateLeaveBalanceComponent } from './dashboard/dashboard-body/main/leave-management/update-leave-balance/update-leave-balance.component';
import { UserComponent } from './user/user.component';

export const routes: Routes =
  [
    { path: 'leave-dashboard', component: LeaveDashboardComponent },
    { path: 'leave-request-history', component: LeaveHistoryComponent },
    { path: 'edit-leave-type', component: EditLeaveTypeComponent },
    { path: 'update-leave-balance', component: UpdateLeaveBalanceComponent },
    { path: 'attendance/calendar', component: CalendarViewComponent },
    { path: '', component: UserComponent }
  ];


