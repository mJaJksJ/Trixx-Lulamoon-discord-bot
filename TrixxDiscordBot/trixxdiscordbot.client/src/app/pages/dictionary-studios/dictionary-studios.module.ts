import { NgModule } from '@angular/core';
import { DictionaryStudiosListComponent } from './list/dictionary-studios-list.component';
import { DictionaryStudiosRoutingModule } from './dictionary-studios-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/modules/shared.module';
import { NbButtonModule, NbCardModule, NbDialogModule } from '@nebular/theme';
import { DictionaryStudiosEditComponent } from './edit/dictionary-studios-edit.component';

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
  ],
})
export class DictionaryStudiosModule {}
