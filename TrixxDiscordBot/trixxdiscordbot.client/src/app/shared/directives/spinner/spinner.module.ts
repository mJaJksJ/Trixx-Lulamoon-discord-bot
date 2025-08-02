import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerDirective } from './spinner.directive';
import { NbSpinnerModule } from '@nebular/theme';

@NgModule({
  declarations: [
    SpinnerDirective,
  ],
  imports: [
    CommonModule,
    NbSpinnerModule,
  ],
  exports: [
    SpinnerDirective,
  ],
})
export class SpinnerModule {}
