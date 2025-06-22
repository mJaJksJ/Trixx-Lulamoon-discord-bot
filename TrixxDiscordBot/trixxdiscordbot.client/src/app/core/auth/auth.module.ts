import { NgModule, } from '@angular/core';
import { NbAuthModule } from '@nebular/auth';
import { TRIXX_AUTH_STRATEGY_NAME, TrixxAuthStrategy } from '../utils/trixx-auth-strategies';
import { TrixxAuthJWTToken } from '../utils/trixx-auth-jwt-token';

@NgModule({
  imports: [],
  declarations: [],
})
export class AuthModule {
  public static get providers() {
    return NbAuthModule.forRoot({
      strategies: [
        TrixxAuthStrategy.setup({
          name: TRIXX_AUTH_STRATEGY_NAME,
          token: {
            class: TrixxAuthJWTToken,
          },
        }),
      ],
    }).providers;
  }
}
