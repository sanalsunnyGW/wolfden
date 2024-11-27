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
import { ProfileComponent } from './profile/profile.component';
import { EmployeeHierarchyTreeComponent } from './employee-hierarchy-tree/employee-hierarchy-tree.component';
import { EmloyeeHierarchyDisplayComponent } from './employee-hierarchy-tree/emloyee-hierarchy-display/emloyee-hierarchy-display.component';
import { MyTeamComponent } from './my-team/my-team.component';



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
            {path:'dashboard',component:MainPageComponent},
            {path:'employee-directory',component: EmployeeDirectoryComponent},
            {path:'leave-dashboard',component:LeaveDashboardComponent},
            {path:'leave-request-history',component:LeaveHistoryComponent},
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
                path:'attendance/calendar',
                component:CalendarViewComponent
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
                path: 'attendance/calendar',
                component: CalendarViewComponent
            },
            {
                path: '',
                component: UserComponent
        
            },
            {
                path: 'profile',
                component: ProfileComponent
            },
            
        ]
    },
    { path: '', redirectTo: '/dashboard/main-page', pathMatch: 'full' }, 

 

];


    



