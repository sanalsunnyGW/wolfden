import { Routes } from '@angular/router';
import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';

export const routes: Routes =
 [
  {path:'leave-dashboard',component:LeaveDashboardComponent},
  {path:'leave-request-history',component:LeaveHistoryComponent}
];
