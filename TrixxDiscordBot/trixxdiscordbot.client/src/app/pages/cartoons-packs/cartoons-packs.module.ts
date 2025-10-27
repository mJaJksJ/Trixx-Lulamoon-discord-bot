import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/modules/shared.module';
import { NbButtonModule, NbCardModule, NbDialogModule, NbIconModule, NbInputModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { CartoonsPacksEditComponent } from './edit/cartoons-pack-edit.component';
import { CartoonsPacksListComponent } from './list/cartoons-packs-list.component';
import { CartoonsPacksRoutingModule } from './cartoons-packs-routing.module';
import { CartoonsPacksCardComponent } from './card/cartoons-pack-card.component';
import { LabelTypeEditComponent } from './card/edit-label-type/label-type-edit.component';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  declarations: [
    CartoonsPacksListComponent,
    CartoonsPacksEditComponent,
    CartoonsPacksCardComponent,
    LabelTypeEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
    CartoonsPacksRoutingModule,
    DragDropModule,
  ],
})
export class CartoonsPacksModule {}
