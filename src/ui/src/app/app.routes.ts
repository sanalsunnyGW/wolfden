import { Routes } from '@angular/router';
import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { AddNewLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/add-new-leave-type/add-new-leave-type.component';
import { LeaveApplicationComponent } from './dashboard/dashboard-body/main/leave-management/leave-application/leave-application.component';
import { UpdateLeaveSettingsComponent } from './dashboard/dashboard-body/main/leave-management/update-leave-settings/update-leave-settings.component';
import { SubordinateLeaveRequestComponent } from './dashboard/dashboard-body/main/leave-management/subordinate-leave-request/subordinate-leave-request.component';
import { EditLeaveRequestComponent } from './dashboard/dashboard-body/main/leave-management/edit-leave-request/edit-leave-request.component';
import { AddLeaveByAdminForEmployeesComponent } from './dashboard/dashboard-body/main/leave-management/add-leave-by-admin-for-employees/add-leave-by-admin-for-employees.component';

export const routes: Routes =
 [
  {path:'leave-dashboard',component:LeaveDashboardComponent},
  {path:'leave-request-history',component:LeaveHistoryComponent},
  {path:'app-add-new-leave-type',component:AddNewLeaveTypeComponent},
  {path:'app-leave-application',component:LeaveApplicationComponent},
  {path:'app-update-leave-settings',component:UpdateLeaveSettingsComponent},
  {path:'app-subordinate-leave-request',component:SubordinateLeaveRequestComponent},
  {path:'app-edit-leave-request',component:EditLeaveRequestComponent},
  {path:'app-add-leave-by-admin-for-employees',component:AddLeaveByAdminForEmployeesComponent}
];
