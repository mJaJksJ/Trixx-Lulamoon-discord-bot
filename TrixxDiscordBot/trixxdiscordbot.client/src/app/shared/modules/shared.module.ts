import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrixxTableModule } from './trixx-table/triix-table.module';

@NgModule({
  exports: [
    CommonModule,
    FormsModule,
    TrixxTableModule,
  ],
})
export class SharedModule {}
