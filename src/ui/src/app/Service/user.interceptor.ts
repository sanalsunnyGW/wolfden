import { HttpInterceptorFn } from '@angular/common/http';

export const userInterceptor: HttpInterceptorFn = (req, next) => {
  const token=localStorage.getItem('token');
  const cloneRequest=req.clone({
    setHeaders:{
      Authorization:`Beare ${token}`
    }
  })
  return next(cloneRequest);
};
