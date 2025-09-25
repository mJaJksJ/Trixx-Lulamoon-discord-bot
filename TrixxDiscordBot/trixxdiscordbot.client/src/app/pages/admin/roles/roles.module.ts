import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbButtonModule, NbCardModule, NbDialogModule, NbIconModule, NbInputModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { RolesListComponent } from './list/roles-list.component';
import { SharedModule } from '../../../shared/modules/shared.module';
import { RolesRoutingModule } from './roles-routing.module';
import { RolesEditComponent } from './edit/roles-edit.component';

@NgModule({
  declarations: [
    RolesListComponent,
    RolesEditComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RolesRoutingModule,
    NbCardModule,
    NbDialogModule.forChild(),
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
  ],
})
export class RolesModule {}
