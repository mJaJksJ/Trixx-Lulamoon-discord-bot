import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrixxTableComponent } from './trixx-table.component';
import { SpinnerModule } from '../../directives/spinner';

@NgModule({
  declarations: [
    TrixxTableComponent,
  ],
  imports: [
    CommonModule,
    SpinnerModule,
  ],
})
export class TrixxTableModule {}
