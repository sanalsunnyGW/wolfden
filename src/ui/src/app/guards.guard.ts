import { CanActivateFn, Router, RouterLink } from '@angular/router';
import { WolfDenService } from './service/wolf-den.service';
import { inject } from '@angular/core';

export const guardsGuard: CanActivateFn = (route, state) => {
  const userService=inject(WolfDenService);
  const isLoggedIn = (userService.userId !== 0);
  const router = inject(Router); 
  if(!isLoggedIn){
    return router.createUrlTree(['/user/login']);  
  }
  return isLoggedIn;
};


