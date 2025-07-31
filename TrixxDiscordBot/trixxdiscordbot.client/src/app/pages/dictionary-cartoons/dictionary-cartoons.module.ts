import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/modules/shared.module';
import { NbButtonModule, NbCardModule, NbDialogModule } from '@nebular/theme';
import { DictionaryCartoonsListComponent } from './list/dictionary-cartoons-list.component';
import { DictionaryCartoonsRoutingModule } from './dictionary-cartoons-routing.module';
import { DictionaryCartoonsEditComponent } from './edit/dictionary-cartoons-edit.component';

@NgModule({
  declarations: [
    DictionaryCartoonsListComponent,
    DictionaryCartoonsEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    DictionaryCartoonsRoutingModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
  ],
})
export class DictionaryCartoonsModule {}
