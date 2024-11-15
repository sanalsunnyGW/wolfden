import { Routes } from '@angular/router';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';
import { DetailedAttendenceComponent } from './Attendence/detailed-attendence/detailed-attendence.component';
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
    {
        path:'detailed-attendence',
        component:DetailedAttendenceComponent
    }
   
];


