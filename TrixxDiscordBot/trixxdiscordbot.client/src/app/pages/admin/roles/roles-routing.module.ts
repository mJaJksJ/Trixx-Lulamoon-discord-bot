import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { RolesListComponent } from './list/roles-list.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: RolesListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RolesRoutingModule { }