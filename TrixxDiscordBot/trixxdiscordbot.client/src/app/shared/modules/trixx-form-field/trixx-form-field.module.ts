import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrixxFormFieldComponent } from './trixx-form-field.component';

@NgModule({
  declarations: [    
    TrixxFormFieldComponent,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    TrixxFormFieldComponent,
  ]
})
export class TrixxFormFieldModule {}