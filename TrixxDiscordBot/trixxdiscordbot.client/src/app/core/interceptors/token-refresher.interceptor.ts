import { Injectable, Inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NbAuthService, NB_AUTH_TOKEN_INTERCEPTOR_FILTER } from '@nebular/auth';
import { catchError, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import {
  TRIXX_AUTH_STRATEGY_NAME,
  TRIXX_LOGIN_PAGE,
} from '../utils/trixx-auth-strategies';

@Injectable()
export class TokenRefresherInterceptor implements HttpInterceptor {
  constructor(
    @Inject(NB_AUTH_TOKEN_INTERCEPTOR_FILTER)
    private readonly filterRequest: (x: string) => boolean,
    private readonly nbAuthService: NbAuthService,
    private readonly router: Router
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (
          err instanceof HttpErrorResponse &&
          err.status === 401 &&
          !this.filterRequest(request.url)
        ) {
          return this.nbAuthService.refreshToken(TRIXX_AUTH_STRATEGY_NAME).pipe(
            switchMap((refreshResult) => {
              if (refreshResult.isFailure()) {
                return this.noNotificationError(err);
              } else {
                return next.handle(request).pipe(
                  catchError((err2) => {
                    if (
                      err2 instanceof HttpErrorResponse &&
                      err2.status === 401
                    ) {
                      this.router.navigateByUrl(TRIXX_LOGIN_PAGE);
                    }
                    return throwError(() => new Error(err2));
                  })
                );
              }
            })
          );
        } else {
          return throwError(() => new Error(err));
        }
      })
    );
  }

  private noNotificationError(error: HttpErrorResponse): Observable<never> {
    const errorNoNotification = error as unknown as INoErrorNotification;
    errorNoNotification.noNotification = true;
    return throwError(() => new Error(error as any));
  }
}

export interface INoErrorNotification {
  noNotification: boolean;
}
