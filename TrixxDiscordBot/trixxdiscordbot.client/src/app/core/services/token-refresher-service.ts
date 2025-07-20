import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { NbAuthResult, NbAuthService } from '@nebular/auth';
import { Observable, of } from 'rxjs';
import { map, shareReplay, switchMap, take } from 'rxjs/operators';
import { NotificationService } from './notification.service';
import { TrixxAuthJWTToken, TrixxAuthJWTTokenPayload } from '../utils/trixx-auth-jwt-token';
import { AuthFailTypes, AuthResultModel } from '../../../api/models';
import { AuthService } from '../../../api/services';
import { ConfirmByTimeModalService } from '../../shared/modules/confirm/confirm-by-time-modal.service';

@Injectable({
  providedIn: 'root',
})
export class TokenRefresherService {
  constructor(private injector: Injector) {}
  private readonly redirectToUrl = '/auth/login';
  private _activeRefreshRequest$: Observable<{ result: NbAuthResult; failType?: AuthFailTypes }> | null = null;
  private openedWindowForExit = false;

  public refresh(ownerStrategyName: string): Observable<NbAuthResult> {
    if (!this._activeRefreshRequest$) {
      const nbAuthService = this.injector.get(NbAuthService);
      this._activeRefreshRequest$ = nbAuthService.getToken().pipe(
        take(1),
        switchMap((data) => {
          if (!(data instanceof TrixxAuthJWTToken)) {
            return of(null);
          }
          const refreshToken = data?.getRefreshToken();
          if (!refreshToken) return of(null);
          const jwtAccess = data.getAccessTokenPayload();
          const userId = jwtAccess['user-id'];
          const apiService = this.injector.get(AuthService);
          const result = apiService.apiAuthRefreshPost({
            userId: userId,
            refreshToken: refreshToken,
          });

          return result;
        }),
        map((result: AuthResultModel | null) => ({
          result: this.mapRefreshResponse(result, ownerStrategyName),
          failType: result?.failType,
        })),
        shareReplay(1),
      );

      const router = this.injector.get(Router);
      this._activeRefreshRequest$.subscribe(async (refreshResult) => {
        this._activeRefreshRequest$ = null;
        if (refreshResult.result.isFailure()) {
          const messages = refreshResult.result.getMessages();
          if (!router.url.startsWith('/auth/login') && !this.openedWindowForExit) {
            if (refreshResult.failType === AuthFailTypes.RefreshTokenInvalid) {
              this.openedWindowForExit = true;
              const confirmModalService = this.injector.get(ConfirmByTimeModalService);
              await confirmModalService.confirmWithAutoClose(
                {
                  title: 'Внимание!',
                  message: messages && messages.length && messages[0] || undefined,
                },
                5,
              );
              this.openedWindowForExit = false;
              router.navigateByUrl(this.redirectToUrl);
            } else {
              if (messages && messages.length) {
                const toastrService = this.injector.get(NotificationService);
                toastrService.showDanger(messages[0], 'Внимание!');
              }
              nbAuthService.logout(ownerStrategyName).toPromise();
              setTimeout(() => {
                router.navigateByUrl(refreshResult.result.getRedirect());
              }, 6000);
            }
          }
        }
      });
    }
    return this._activeRefreshRequest$.pipe(map((x) => x.result));
  }

  private mapRefreshResponse(
    x: AuthResultModel | null,
    ownerStrategyName: string,
  ): NbAuthResult {
    if (x === null) {
      return new NbAuthResult(false, false, this.redirectToUrl, ['Сессия истекла, авторизуйтесь снова.']);
    }

    if (x.success) {
      const tokenParams: TrixxAuthJWTTokenPayload = {
        access_token: x.accessToken,
        refresh_token: x.refreshToken,
        permissions: x.permissions,
      };
      const token = new TrixxAuthJWTToken(tokenParams, ownerStrategyName, undefined);
      return new NbAuthResult(true, x, null, null, null, token);
    } else {
      return new NbAuthResult(
        false,
        false,
        this.redirectToUrl,
        [x.error],
        [x.error],
      );
    }
  }
}
