import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import {
  NbActionsModule,
  NbLayoutModule,
  NbMenuModule,
  NbSidebarModule,
  NbUserModule,
  NbContextMenuModule,
  NbButtonModule,
  NbSelectModule,
  NbIconModule,
  NbThemeModule,
  NbCardModule,
  COSMIC_THEME,
} from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { PortalModule } from '@angular/cdk/portal';
import { OneColumnLayoutComponent, RouterOutletLayoutComponent } from './layouts';
import { RightSidebarContentWrapComponent } from './components/right-sidebar-content-wrap';
import { RightSidebarTogglerComponent } from './components/right-sidebar-toggler';
import { TrixxHeaderComponent } from './components/header';

const MODULES = [
  NbLayoutModule,
  NbMenuModule,
  NbUserModule,
  NbActionsModule,
  NbSidebarModule,
  NbContextMenuModule,
  NbButtonModule,
  NbSelectModule,
  NbIconModule,
  NbEvaIconsModule,
  ReactiveFormsModule,
  PortalModule,
  NbCardModule,
];

const COMPONENTS: any[] = [
  RouterOutletLayoutComponent,
  OneColumnLayoutComponent,
  RightSidebarContentWrapComponent,
  RightSidebarTogglerComponent,
  TrixxHeaderComponent,
];
const PIPES: any[] = [];

@NgModule({
  imports: [CommonModule, ...MODULES, RouterModule],
  exports: [CommonModule, ...PIPES, ...COMPONENTS],
  declarations: [...COMPONENTS, ...PIPES],
})
export class ThemeModule {
  static forRoot(): ModuleWithProviders<ThemeModule> {
    return {
      ngModule: ThemeModule,
      providers: [
        ...NbThemeModule.forRoot(
          {
            name: 'cosmic',
          },
          [COSMIC_THEME]
        ).providers!,
      ],
    };
  }
}
