import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NbButtonModule, NbIconModule, NbInputModule, NbSelectModule } from '@nebular/theme';
import { TrixxFormArrayHelperComponent } from './trixx-form-array-helper.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TrixxFormFieldModule } from '../trixx-form-field/trixx-form-field.module';

@NgModule({
  declarations: [
    TrixxFormArrayHelperComponent,
  ],
  imports: [
    CommonModule,
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    NbIconModule,
    ReactiveFormsModule,
    TrixxFormFieldModule,
    NbSelectModule,
  ],
  exports: [
    TrixxFormArrayHelperComponent,
  ]
})
export class TrixxFormArrayHelperModule {}
