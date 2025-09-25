import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbButtonModule, NbCardModule, NbDialogModule, NbIconModule, NbInputModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersListComponent } from './list/users-list.component';
import { SharedModule } from '../../../shared/modules/shared.module';
import { UsersRoutingModule } from './users-routing.module';
import { DictionaryUsersEditComponent } from './edit/users-edit.component';

@NgModule({
  declarations: [
    UsersListComponent,
    DictionaryUsersEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    UsersRoutingModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
  ],
})
export class UsersModule {}
