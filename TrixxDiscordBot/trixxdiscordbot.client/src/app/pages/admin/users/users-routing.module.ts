import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { UsersListComponent } from './list/users-list.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: UsersListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UsersRoutingModule { }