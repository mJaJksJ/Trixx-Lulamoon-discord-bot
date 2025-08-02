import {
  ComponentFactoryResolver,
  Directive,
  ElementRef,
  Renderer2,
  ViewContainerRef,
  Input,
} from '@angular/core';
import { NbSpinnerDirective, NbComponentStatus } from '@nebular/theme';

@Directive({
  selector: '[appSpinner]',
  standalone: false,
})
export class SpinnerDirective extends NbSpinnerDirective {
  @Input() set appSpinner(loading: boolean | null) {
    this.nbSpinner = !!loading;
  }

  @Input() set appSpinnerStatus(status: NbComponentStatus) {
    this.spinnerStatus = status;
  }

  constructor(
    directiveView: ViewContainerRef,
    componentFactoryResolver: ComponentFactoryResolver,
    renderer: Renderer2,
    directiveElement: ElementRef,
  ) {
    super(directiveView, componentFactoryResolver, renderer, directiveElement);
    this.spinnerMessage = 'Загрузка...';
    this.spinnerSize = 'large';
    this.spinnerStatus = 'primary';
  }
}
