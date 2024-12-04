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
import { guardsGuard } from './guards.guard';
import { WeeklyAttendanceComponent } from './dashboard/dashboard-body/main/attendance-module/weekly-attendance/weekly-attendance.component';
import { DailyAttendenceComponent } from './dashboard/dashboard-body/main/attendance-module/daily-attendence/daily-attendence.component';
import { MonthlyReportComponent } from './dashboard/dashboard-body/main/attendance-module/monthly-report/monthly-report.component';
import { SubordinatesComponent } from './dashboard/dashboard-body/main/attendance-module/subordinates/subordinates.component';
import { EditLeaveTypeComponent } from './dashboard/dashboard-body/main/leave-management/edit-leave-type/edit-leave-type.component';
import { UpdateLeaveBalanceComponent } from './dashboard/dashboard-body/main/leave-management/update-leave-balance/update-leave-balance.component';
import { ProfileComponent } from './profile/profile.component';
import { AttendanceHistoryComponent } from './dashboard/dashboard-body/main/attendance-module/attendance-history/attendance-history.component';

export const routes: Routes = [
{
        path: 'user', 
        component: UserComponent,
        children:[
            {path: 'check-user', component: CheckUserComponent},
            {path: 'sign-in', component: SigninComponent},
            {path: 'login', component: LoginComponent},
        ]
    },
    {
        path:'portal',
        component:DashboardComponent,
        children:[
            { path: '', redirectTo: '/dashboard', pathMatch: 'full' }, 
            {path:'dashboard',component:MainPageComponent},
            {path:'employee-directory',component: EmployeeDirectoryComponent},
            {path:'main-page',component:MainPageComponent},
            {path:'employee-directory',component: EmployeeDirectoryComponent},
            {path:'leave-dashboard',component:LeaveDashboardComponent},
            {path:'leave-request-history',component:LeaveHistoryComponent},
            {path:'attendance/daily/:attendanceDate',component:DailyAttendenceComponent},
            {path:'attendance/weekly',component:WeeklyAttendanceComponent},
            {path:'attendance/monthly',component:MonthlyReportComponent},
            {path:'attendance/subordinates',component:SubordinatesComponent} ,  
            {path:'attendance/history',component:AttendanceHistoryComponent}, 
            { path: 'edit-leave-type', component: EditLeaveTypeComponent },
            { path: 'update-leave-balance', component: UpdateLeaveBalanceComponent },
            {path:'attendance/attendance-history',component:AttendanceHistoryComponent},
            {
                path: 'company-hierarchy',
                component: EmployeeHierarchyTreeComponent,
                canActivate: [guardsGuard]
            },
            {
                path: 'employee-display',
                component: EmloyeeHierarchyDisplayComponent,
                canActivate: [guardsGuard]
            },
            {
                path: 'my-team',
                component: MyTeamComponent,
                canActivate: [guardsGuard]
            },
            {
                path:'attendance/calendar',
                component:CalendarViewComponent,
                
            },
            {
                path: 'leave-dashboard',
                component: LeaveDashboardComponent,
                canActivate: [guardsGuard]
            },
            {
                path: 'leave-request-history',
                component: LeaveHistoryComponent,
    
            },
            {
                path: 'profile',
                component: ProfileComponent,
                canActivate: [guardsGuard]
            },
            
        ]
    },

    { path: '', redirectTo: '/portal/dashboard', pathMatch: 'full' }, 


];
