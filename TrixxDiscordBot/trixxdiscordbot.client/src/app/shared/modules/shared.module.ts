import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrixxTableModule } from './trixx-table/trixx-table.module';
import { TrixxFormFieldModule } from './trixx-form-field/trixx-form-field.module';
import { SpinnerModule } from '../directives/spinner';
import { TrixxFormArrayHelperModule } from './trixx-form-array-helper/trixx-form-array-helper.module';

@NgModule({
  exports: [
    CommonModule,
    FormsModule,
    TrixxTableModule,
    TrixxFormFieldModule,
    SpinnerModule,
    TrixxFormArrayHelperModule,
  ],
  declarations: [],
})
export class SharedModule {}
