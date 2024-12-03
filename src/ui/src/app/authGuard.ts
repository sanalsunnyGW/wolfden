import { CanActivateFn, Router, RouterLink } from '@angular/router';
import { WolfDenService } from './service/wolf-den.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const userService=inject(WolfDenService);
  userService.checkExpiry();
  const isLoggedIn =  !!localStorage.getItem('token');;
  const router = inject(Router); 
  if(!isLoggedIn){
    return router.createUrlTree(['/user/login']);  
  }
  return isLoggedIn;
};


