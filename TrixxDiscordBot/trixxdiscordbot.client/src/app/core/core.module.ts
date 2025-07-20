import {
  LOCALE_ID,
  ModuleWithProviders,
  NgModule,
  Optional,
  SkipSelf,
} from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import { NbTokenLocalStorage, NbTokenStorage } from '@nebular/auth';
import { NotificationService } from './services/notification.service';
import { ErrorHandlerModule } from './errors/error-handler.module';
import { AuthModule } from './auth/auth.module';
import { TrixxAccessCheckerService } from './services/trixx-access-checker.service';
import { TrixxAuthStrategy } from './utils/trixx-auth-strategies';
import { LayoutService } from './utils/layout.service';

registerLocaleData(localeRu);

@NgModule({
  imports: [CommonModule, ErrorHandlerModule],
  exports: [AuthModule],
  declarations: [],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule has already been loaded. Import Core modules in the AppModule only.'
      );
    }
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        ...AuthModule.providers!,
        { provide: NbTokenStorage, useClass: NbTokenLocalStorage },
        {
          provide: LOCALE_ID,
          useValue: 'ru',
        },
        TrixxAccessCheckerService,
        LayoutService,
        NotificationService,
        TrixxAuthStrategy,
        NotificationService,
      ],
    };
  }
}
