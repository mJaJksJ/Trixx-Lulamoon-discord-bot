import {
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { ThemeModule } from './themes/theme.module';
import { SharedModule } from './shared/modules/shared.module';
import {
  NbAuthJWTInterceptor,
  NB_AUTH_TOKEN_INTERCEPTOR_FILTER,
} from '@nebular/auth';
import { TokenRefresherInterceptor } from './core/interceptors/token-refresher.interceptor';
import { ApiModule } from './../api/api.module';
import { ApiConfiguration } from './../api/api-configuration';

const excludeUrls = ['/api/Auth/refresh', '/api/Auth/send-reset-password-mail'];
const filterInterceptorRequest = (request: any) =>
  excludeUrls.includes(request.url);

@NgModule({
  declarations: [AppComponent],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule.forRoot(),
    ThemeModule.forRoot(),
    ApiModule,
    SharedModule,
  ],
  providers: [
    {
      provide: NB_AUTH_TOKEN_INTERCEPTOR_FILTER,
      useValue: filterInterceptorRequest,
    },
    // TokenRefresherInterceptor должен быть перед NbAuthJWTInterceptor
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenRefresherInterceptor,
      multi: true,
    },
    { provide: HTTP_INTERCEPTORS, useClass: NbAuthJWTInterceptor, multi: true },
    {
      provide: ApiConfiguration,
      useValue: { rootUrl: '' } as ApiConfiguration,
    },
  ],
})
export class AppModule {}
