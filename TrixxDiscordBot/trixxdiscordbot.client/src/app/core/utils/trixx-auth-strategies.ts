import {
  NbAuthStrategyClass,
  NbAuthStrategyOptions,
  NbAuthStrategy,
  NbAuthResult,
} from '@nebular/auth';
import { map, Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../api/services';
import { AuthResultModel } from '../../../api/models';
import { TrixxAuthJWTToken, TrixxAuthJWTTokenPayload } from './trixx-auth-jwt-token';

export const TRIXX_AUTH_STRATEGY_NAME = 'email';
export const TRIXX_LOGIN_PAGE = '/auth/login';

@Injectable()
export class TrixxAuthStrategy extends NbAuthStrategy {
  constructor(
    private readonly apiService: AuthService,
    private readonly route: ActivatedRoute
  ) {
    super();
  }

  authenticate(data?: any): Observable<NbAuthResult> {
    const redirectTo =
      this.route.snapshot.queryParams &&
      this.route.snapshot.queryParams['redirectTo'];

    return this.apiService
      .apiAuthLoginPost({ body: { email: data.email, password: data.password } })
      .pipe(map((x) => this.mapLoginResponse(x, redirectTo)));
  }

  logout(): Observable<NbAuthResult> {
    return this.apiService
      .apiAuthLogoutDelete()
      .pipe(map(() => new NbAuthResult(true, null, TRIXX_LOGIN_PAGE)));
  }

  override register(): Observable<NbAuthResult> {
    throw new Error('Method not implemented.');
  }

  override requestPassword(): Observable<NbAuthResult> {
    throw new Error('Method not implemented.');
  }
  
  override resetPassword(): Observable<NbAuthResult> {
    throw new Error('Method not implemented.');
  }

  override refreshToken(): Observable<NbAuthResult> {
    throw new Error('Method not implemented.');
  }

  static setup(
    opts: NbAuthStrategyOptions
  ): [NbAuthStrategyClass, NbAuthStrategyOptions] {
    return [TrixxAuthStrategy, opts];
  }

  private mapLoginResponse(
    x: AuthResultModel,
    redirectTo: string
  ): NbAuthResult {
    if (x && x.success) {
      const tokenParams: TrixxAuthJWTTokenPayload = {
        access_token: x.accessToken!,
        refresh_token: x.refreshToken!,
        permissions: x.permissions!,
      };
      const token = new TrixxAuthJWTToken(tokenParams, this.options.name, undefined);
      const redirectToUrl = redirectTo ? redirectTo : '/main';
      return new NbAuthResult(true, x, redirectToUrl, null, null, token);
    } else {
      return new NbAuthResult(false, false, false, [x.error], [x.error]);
    }
  }
}
