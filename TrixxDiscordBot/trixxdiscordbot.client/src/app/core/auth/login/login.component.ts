import {
  Component,
  ChangeDetectorRef,
  OnInit,
  OnDestroy,
  Inject,
} from '@angular/core';
import {
  NbLoginComponent,
  NbAuthResult,
  NbAuthService,
  NB_AUTH_OPTIONS,
  NbAuthOptions,
} from '@nebular/auth';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  standalone: false,
  templateUrl: './login.component.html',
})
export class LoginComponent extends NbLoginComponent implements OnInit, OnDestroy {
  private readonly _$subscription = new Subscription();

  constructor(
    @Inject(NB_AUTH_OPTIONS) options: NbAuthOptions,
    service: NbAuthService,
    cd: ChangeDetectorRef,
    router: Router,
  ) {
    super(service, options, cd, router);
  }

  ngOnInit(): void { }

  ngOnDestroy(): void {
    this._$subscription.unsubscribe();
  }

  override login(): void {
    this.errors = [];
    this.messages = [];
    this.submitted = true;

    this.service
      .authenticate(this.strategy, this.user)
      .subscribe((result: NbAuthResult) => this.handleAuthResult(result));
  }

  private handleAuthResult(result: NbAuthResult) {
    this.submitted = false;

    if (result.isSuccess()) {
      this.messages = result.getMessages();
    } else {
      this.errors = result.getErrors();
    }

    const redirect = result.getRedirect();
    if (redirect) {
      setTimeout(() => {
        if (redirect.startsWith('http://') || redirect.startsWith('https://')) {
          window.location.href = redirect;
          return;
        } else {
          return this.router.navigateByUrl(redirect);
        }
      }, this.redirectDelay);
    }

    this.cd.detectChanges();
  }
}
