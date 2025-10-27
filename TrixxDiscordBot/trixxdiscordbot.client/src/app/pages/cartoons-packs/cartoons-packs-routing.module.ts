import { NgModule } from '@angular/core';
import { CartoonsPacksListComponent } from './list/cartoons-packs-list.component';
import { RouterModule, Routes } from '@angular/router';
import { CartoonsPacksCardComponent } from './card/cartoons-pack-card.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: CartoonsPacksListComponent,
    },
    {
        path: 'card/:id',
        pathMatch: 'full',
        component: CartoonsPacksCardComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CartoonsPacksRoutingModule {}
