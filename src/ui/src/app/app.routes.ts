import { Routes } from '@angular/router';
import { LeaveDashboardComponent } from './dashboard/dashboard-body/main/leave-management/leave-dashboard/leave-dashboard.component';
import { LeaveHistoryComponent } from './dashboard/dashboard-body/main/leave-management/leave-history/leave-history.component';
import { ProfileComponent } from './profile/profile.component';
import { EmployeeHierarchyComponent } from './employee-hierarchy/employee-hierarchy.component';

export const routes: Routes = [
    {
        path:'profile',
        component:ProfileComponent
    },
    {
        path:'leave-dashboard',
        component:LeaveDashboardComponent
    },
    {
        path:'leave-request-history',
        component:LeaveHistoryComponent
    },
    {
        path:'employee-hierarchy',
        component:EmployeeHierarchyComponent
    },
   
];
