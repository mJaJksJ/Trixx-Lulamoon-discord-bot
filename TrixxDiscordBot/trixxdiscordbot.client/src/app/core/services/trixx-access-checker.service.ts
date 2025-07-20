import { Injectable } from '@angular/core';
import { Permission } from '../../../api/models/permission';
import { NbTokenService } from '@nebular/auth';
import { BehaviorSubject, concat, Observable, ReplaySubject } from 'rxjs';
import { distinctUntilChanged, map } from 'rxjs/operators';
import { TrixxAuthJWTToken, TrixxAuthJWTTokenPayload } from '../utils/trixx-auth-jwt-token';

@Injectable({
  providedIn: 'root',
})
export class TrixxAccessCheckerService {
  private readonly permissions$: BehaviorSubject<Permission[]>;
  public readonly isObserver$ = new ReplaySubject<boolean>(1);
  public readonly changes$: Observable<void>;

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

    this.changes$ = this.permissions$.pipe(
      distinctUntilChanged((a, b) => a.join(';') === b.join(';')),
      map(() => undefined),
    );
  }

  public isGranted(permission: string) {
    return this.permissions$.getValue().includes(permission as Permission);
  }
}
