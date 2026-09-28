import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

const loginPath = '/bff/login';

export const sessionInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const returnUrl = window.location.pathname + window.location.search;
        window.location.assign(`${loginPath}?returnUrl=${encodeURIComponent(returnUrl)}`);
      }

      return throwError(() => error);
    })
  );
