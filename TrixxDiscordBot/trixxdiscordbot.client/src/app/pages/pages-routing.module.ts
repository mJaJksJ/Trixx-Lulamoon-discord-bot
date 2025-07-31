import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { PagesComponent } from './pages.component';

const routes: Routes = [
  {
    path: '',
    component: PagesComponent,
    children: [
      {
        path: 'main',
        loadChildren: () =>
          import('./main/main.module').then((m) => m.MainModule),
      },
      {
        path: 'dictionary-studios',
        loadChildren: () =>
          import('./dictionary-studios/dictionary-studios.module').then((m) => m.DictionaryStudiosModule),
      },
      {
        path: 'dictionary-cartoons',
        loadChildren: () =>
          import('./dictionary-cartoons/dictionary-cartoons.module').then((m) => m.DictionaryCartoonsModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {}
