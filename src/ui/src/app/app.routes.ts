import { Routes } from '@angular/router';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';

import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { ProfileComponent } from './profile/profile.component';
import { EmployeeHierarchyTreeComponent } from './employee-hierarchy-tree/employee-hierarchy-tree.component';
import { EmloyeeHierarchyDisplayComponent } from './employee-hierarchy-tree/emloyee-hierarchy-display/emloyee-hierarchy-display.component';
import { MyTeamComponent } from './my-team/my-team.component';
import { UserComponent } from './user/user.component';
import { WeeklyAttendanceComponent } from './dashboard/dashboard-body/main/attendance-module/weekly-attendance/weekly-attendance.component';


export const routes: Routes = [
    {
        path: 'profile',
        component: ProfileComponent
    },
    {
        path: 'leave-dashboard',
        component: LeaveDashboardComponent
    },
    {
        path: 'leave-request-history',
        component: LeaveHistoryComponent
    },
    {
        path: 'company-hierarchy',
        component: EmployeeHierarchyTreeComponent
    },
    {
        path: 'employee-display',
        component: EmloyeeHierarchyDisplayComponent
    },
    {
        path: 'my-team',
        component: MyTeamComponent
    },

    {
        path: 'attendance/calendar',
        component: CalendarViewComponent
    },
    {
        path: '',
        component: UserComponent

    },
    {path:'leave-dashboard',component:LeaveDashboardComponent},
    {path:'leave-request-history',component:LeaveHistoryComponent},
    {path:'attendance/weekly',component:WeeklyAttendanceComponent}

    
   
];


