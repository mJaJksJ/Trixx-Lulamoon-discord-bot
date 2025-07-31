import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { DictionaryCartoonsListComponent } from './list/dictionary-cartoons-list.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: DictionaryCartoonsListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DictionaryCartoonsRoutingModule { }