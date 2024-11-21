import { Routes } from '@angular/router';
import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { AddNewLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/add-new-leave-type/add-new-leave-type.component';
import { LeaveApplicationComponent } from './dashboard/dashboard-body/main/leave-management/leave-application/leave-application.component';

export const routes: Routes =
 [
  {path:'leave-dashboard',component:LeaveDashboardComponent},
  {path:'leave-request-history',component:LeaveHistoryComponent},
  {path:'app-add-new-leave-type',component:AddNewLeaveTypeComponent},
  {path:'app-leave-application',component:LeaveApplicationComponent}
];
