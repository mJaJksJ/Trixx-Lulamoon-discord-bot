import { Injectable } from '@angular/core';
import { Permission } from '../../../api/models/permission';
import { NbTokenService } from '@nebular/auth';
import { BehaviorSubject, concat, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { TrixxAuthJWTToken, TrixxAuthJWTTokenPayload } from '../utils/trixx-auth-jwt-token';

@Injectable({
  providedIn: 'root',
})
export class TrixxAccessCheckerService {
  private readonly permissions$: BehaviorSubject<Permission[]>;
  public readonly isObserver$ = new ReplaySubject<boolean>(1);

  constructor(tokenService: NbTokenService) {
    this.permissions$ = new BehaviorSubject<Permission[]>([]);

    const tokenPayload$ = concat(tokenService.get(), tokenService.tokenChange()).pipe(
      map(x => {
        if (x instanceof TrixxAuthJWTToken) {
          const payload = x.getPayload();
          return payload as TrixxAuthJWTTokenPayload;
        } else {
          return null;
        }
      }),
    );

    const permissions$ = tokenPayload$.pipe(
      map((x) => x ? x.permissions : []),
    );
    permissions$.subscribe(this.permissions$);
  }
}
