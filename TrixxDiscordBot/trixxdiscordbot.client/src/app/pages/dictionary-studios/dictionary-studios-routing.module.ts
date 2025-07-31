import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { DictionaryStudiosListComponent } from './list/dictionary-studios-list.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: DictionaryStudiosListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DictionaryStudiosRoutingModule { }