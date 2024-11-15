import { Routes } from '@angular/router';
import { CalendarViewComponent } from './dashboard/dashboard-body/main/attendance-module/calendar-view/calendar-view.component';

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
    
   
];


