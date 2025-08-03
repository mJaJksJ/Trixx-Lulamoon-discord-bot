import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrixxTableComponent } from './trixx-table.component';
import { SpinnerModule } from '../../directives/spinner';
import { NbButtonModule, NbIconModule } from '@nebular/theme';

@NgModule({
  declarations: [
    TrixxTableComponent,
  ],
  imports: [
    CommonModule,
    SpinnerModule,
    NbIconModule,
    NbButtonModule,
  ],
})
export class TrixxTableModule {}
