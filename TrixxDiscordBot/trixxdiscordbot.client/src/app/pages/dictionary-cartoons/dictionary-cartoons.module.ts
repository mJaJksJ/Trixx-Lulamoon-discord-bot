import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/modules/shared.module';
import { NbButtonModule, NbCardModule, NbDialogModule, NbIconModule, NbInputModule, NbSelectModule } from '@nebular/theme';
import { DictionaryCartoonsListComponent } from './list/dictionary-cartoons-list.component';
import { DictionaryCartoonsRoutingModule } from './dictionary-cartoons-routing.module';
import { DictionaryCartoonsEditComponent } from './edit/dictionary-cartoons-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DictionaryCartoonsFormDefaultComponent } from './form-default/dictionary-cartoons-form-default.component';

@NgModule({
  declarations: [
    DictionaryCartoonsListComponent,
    DictionaryCartoonsEditComponent,
    DictionaryCartoonsFormDefaultComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    DictionaryCartoonsRoutingModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
    NbSelectModule,
  ],
})
export class DictionaryCartoonsModule {}
