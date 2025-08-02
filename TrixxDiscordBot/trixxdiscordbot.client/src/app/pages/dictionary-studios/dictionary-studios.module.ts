import { NgModule } from '@angular/core';
import { DictionaryStudiosListComponent } from './list/dictionary-studios-list.component';
import { DictionaryStudiosRoutingModule } from './dictionary-studios-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/modules/shared.module';
import { NbButtonModule, NbCardModule, NbDialogModule, NbIconModule, NbInputModule } from '@nebular/theme';
import { DictionaryStudiosEditComponent } from './edit/dictionary-studios-edit.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DictionaryStudiosListComponent,
    DictionaryStudiosEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    DictionaryStudiosRoutingModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
  ],
})
export class DictionaryStudiosModule {}
