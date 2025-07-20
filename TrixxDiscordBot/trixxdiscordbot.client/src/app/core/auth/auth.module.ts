import { NgModule, } from '@angular/core';
import { NbAuthModule } from '@nebular/auth';
import { TRIXX_AUTH_STRATEGY_NAME, TrixxAuthStrategy } from '../utils/trixx-auth-strategies';
import { TrixxAuthJWTToken } from '../utils/trixx-auth-jwt-token';
import { AuthRoutingModule, routedComponents } from './auth-routing.module';
import { CommonModule } from '@angular/common';
import { NbAlertModule, NbButtonModule, NbCardModule, NbInputModule, NbLayoutModule, NbToastrModule } from '@nebular/theme';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/modules/shared.module';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    NbAlertModule,
    NbCardModule,
    NbLayoutModule,
    NbInputModule,
    NbButtonModule,
    AuthRoutingModule,
    NbAuthModule,
    SharedModule,
    ReactiveFormsModule,
    NbToastrModule,
  ],
  declarations: [
    ...routedComponents,
  ],
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
