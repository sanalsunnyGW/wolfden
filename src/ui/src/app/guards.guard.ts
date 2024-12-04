import { CanActivateFn, Router, RouterLink } from '@angular/router';
import { WolfDenService } from './service/wolf-den.service';
import { inject } from '@angular/core';

export const guardsGuard: CanActivateFn = (route, state) => {
return true;
  
};


