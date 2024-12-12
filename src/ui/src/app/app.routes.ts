import { Routes } from '@angular/router';
import { CheckUserComponent } from './user/check-user/check-user.component';
import { SigninComponent } from './user/signin/signin.component';
import { LoginComponent } from './user/login/login.component';
import { UserComponent } from './user/user.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EmployeeDirectoryComponent } from './dashboard/dashboard-body/main/employee-directory/employee-directory.component';
import { MainPageComponent } from './dashboard/dashboard-body/main/main-page/main-page.component';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';
import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { EmployeeHierarchyTreeComponent } from './employee-hierarchy-tree/employee-hierarchy-tree.component';
import { EmloyeeHierarchyDisplayComponent } from './employee-hierarchy-tree/emloyee-hierarchy-display/emloyee-hierarchy-display.component';
import { MyTeamComponent } from './my-team/my-team.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { authGuard } from './authGuard';
import { WeeklyAttendanceComponent } from './dashboard/dashboard-body/main/attendance-module/weekly-attendance/weekly-attendance.component';
import { DailyAttendenceComponent } from './dashboard/dashboard-body/main/attendance-module/daily-attendence/daily-attendence.component';
import { MonthlyReportComponent } from './dashboard/dashboard-body/main/attendance-module/monthly-report/monthly-report.component';
import { SubordinatesComponent } from './dashboard/dashboard-body/main/attendance-module/subordinates/subordinates.component';
import { EditLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/edit-leave-type/edit-leave-type.component';
import { UpdateLeaveBalanceComponent } from './dashboard/dashboard-body/main/leave-management/update-leave-balance/update-leave-balance.component';
import { ProfileComponent } from './profile/profile.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { AddNewLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/add-new-leave-type/add-new-leave-type.component';
import { LeaveApplicationComponent } from './dashboard/dashboard-body/main/leave-management/leave-application/leave-application.component';
import { UpdateLeaveSettingsComponent } from './dashboard/dashboard-body/main/leave-management/update-leave-settings/update-leave-settings.component';
import { SubordinateLeaveRequestComponent } from './dashboard/dashboard-body/main/leave-management/subordinate-leave-request/subordinate-leave-request.component';
import { EditLeaveRequestComponent } from './dashboard/dashboard-body/main/leave-management/edit-leave-request/edit-leave-request.component';
import { AddLeaveByAdminForEmployeesComponent } from './dashboard/dashboard-body/main/leave-management/add-leave-by-admin-for-employees/add-leave-by-admin-for-employees.component';
import { AttendanceHistoryComponent } from './dashboard/dashboard-body/main/attendance-module/attendance-history/attendance-history.component';
import { ResetPasswordComponent } from './user/reset-password/reset-password.component';
import { CheckPasswordComponent } from './user/check-password/check-password.component';
import { AdminAddHolidayComponent } from './admin-add-holiday/admin-add-holiday.component';

export const routes: Routes = [
    {
        path: 'user',
        component: UserComponent,
        children: [
            { path: '', redirectTo: 'login', pathMatch: 'full' },
            { path: 'check-user', component: CheckUserComponent },
            { path: 'sign-in', component: SigninComponent },
            { path: 'login', component: LoginComponent },
            { path:'reset-password',component:ResetPasswordComponent},
            { path:'check-password', component:CheckPasswordComponent}
        ]
    },
    {
        path: 'portal',
        component: DashboardComponent,
        children: [
            { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
            { path:'dashboard',component:MainPageComponent, canActivate: [authGuard]},
            { path:'employee-directory',component: EmployeeDirectoryComponent, canActivate: [authGuard]},
            { path:'leave-dashboard',component:LeaveDashboardComponent, canActivate: [authGuard]},
            { path:'leave-request-history',component:LeaveHistoryComponent, canActivate: [authGuard]},
            { path: 'employee-directory', component: EmployeeDirectoryComponent, canActivate: [authGuard] },
            { path: 'leave-dashboard', component: LeaveDashboardComponent, canActivate: [authGuard] },
            { path: 'leave-request-history', component: LeaveHistoryComponent, canActivate: [authGuard] },
            { path: 'attendance/daily/:attendanceDate', component: DailyAttendenceComponent, canActivate: [authGuard] },
            { path: 'attendance/weekly', component: WeeklyAttendanceComponent, canActivate: [authGuard] },
            { path: 'attendance/monthly', component: MonthlyReportComponent, canActivate: [authGuard] },
            { path: 'attendance/subordinates', component: SubordinatesComponent, canActivate: [authGuard] },
            { path: 'edit-leave-type', component: EditLeaveTypeComponent, canActivate: [authGuard] },
            { path: 'update-leave-balance', component: UpdateLeaveBalanceComponent, canActivate: [authGuard] },
            { path:'attendance/attendance-history',component:AttendanceHistoryComponent, canActivate: [authGuard]},
            { path: 'company-hierarchy',component: EmployeeHierarchyTreeComponent, canActivate: [authGuard]},                                          
            { path: 'employee-display', component: EmloyeeHierarchyDisplayComponent, canActivate: [authGuard]},
            { path: 'my-team', component: MyTeamComponent, canActivate: [authGuard] },
            { path: 'attendance/calendar',  component: CalendarViewComponent, canActivate: [authGuard] },
            { path: 'leave-dashboard', component: LeaveDashboardComponent, canActivate: [authGuard] },
            { path: 'leave-request-history', component: LeaveHistoryComponent,  },        
            { path: 'profile', component: ProfileComponent, canActivate: [authGuard]  },
            { path: 'admin-dashboard',component: AdminDashboardComponent,canActivate: [authGuard] },
            { path: 'employee-role', component: EditRoleComponent,canActivate: [authGuard] },
            { path: 'add-new-leave-type', component: AddNewLeaveTypeComponent, canActivate: [authGuard]},
            { path: 'leave-application', component: LeaveApplicationComponent, canActivate: [authGuard]},
            { path: 'update-leave-settings', component: UpdateLeaveSettingsComponent, canActivate: [authGuard]}, 
            { path: 'subordinate-leave-request',component: SubordinateLeaveRequestComponent, canActivate: [authGuard]},   
            { path: 'edit-leave-request/:leaveRequestId', component: EditLeaveRequestComponent, canActivate: [authGuard]},   
            { path: 'add-leave-by-admin-for-employees', component: AddLeaveByAdminForEmployeesComponent, canActivate: [authGuard]},
            { path:'admin-add-holiday',component:AdminAddHolidayComponent, canActivate: [authGuard]}         
        ]
    },
    { path: '', redirectTo: '/portal/dashboard', pathMatch: 'full' },
];
