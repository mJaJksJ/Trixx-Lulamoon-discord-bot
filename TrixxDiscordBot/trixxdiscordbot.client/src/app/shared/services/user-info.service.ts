import { Injectable, OnDestroy } from '@angular/core';
import { Observable, ReplaySubject, Subscription } from 'rxjs';
import { NbTokenService } from '@nebular/auth';
import { filter, switchMap } from 'rxjs/operators';
import { UserProfileService } from '../../../api/services';
import { UserProfileModel } from '../../../api/models';

@Injectable({
  providedIn: 'root',
})
export class UserInfoService implements OnDestroy {
  public readonly data$: Observable<UserProfileModel>;

  private readonly _data$: ReplaySubject<UserProfileModel>;
  private readonly _$subscription: Subscription;

  constructor(tokenService: NbTokenService, profileService: UserProfileService) {
    this._data$ = new ReplaySubject<UserProfileModel>(1);
    this.data$ = this._data$.asObservable();

    this._$subscription = tokenService
      .tokenChange()
      .pipe(
        filter(x =>
        {
          return x.isValid();
        }),
        switchMap(() => profileService.apiUserProfileGet()))
      .subscribe(this._data$);
  }

  ngOnDestroy(): void {
    this._$subscription.unsubscribe();
    this._data$.complete();
  }
}
