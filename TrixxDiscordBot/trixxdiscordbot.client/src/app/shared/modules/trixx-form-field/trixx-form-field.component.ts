import { AfterContentInit, ChangeDetectorRef, Component, ContentChild, Input } from '@angular/core';
import { NgControl } from '@angular/forms';
import { NbInputDirective } from '@nebular/theme';
import { concat, defer, map, Observable, of } from 'rxjs';

@Component({
  selector: 'app-trixx-form-field',
  standalone: false,
  templateUrl: './trixx-form-field.component.html',
  styleUrl: './trixx-form-field.component.scss'
})
export class TrixxFormFieldComponent implements AfterContentInit {
  @ContentChild('appFormField', { read: NgControl }) private ngControl!: NgControl;
  @ContentChild(NbInputDirective) nbInput?: NbInputDirective;
  @Input() public label = '';
  public error$ = new Observable<boolean>();

  constructor(private readonly cdr: ChangeDetectorRef) {}

  ngAfterContentInit() {
    if (!this.ngControl) {
      throw new Error(`Not control`);
    }

    const status$ = concat(
      defer(() => of(this.ngControl.status)),
      this.ngControl.statusChanges!,
    );

    this.error$ = status$.pipe(
      map((status) => {
        const isError = status === 'INVALID';
        if (this.nbInput) {
          this.nbInput.status = isError ? 'danger' : 'basic';
        }
        return isError;
      }),
    );
    this.cdr.detectChanges();
  }

  public get error(): string {
    const errors = this.ngControl.control?.errors;

    if (!errors) {
      return '';
    }

    if (errors['required']) {
      return 'Обязательное поле';
    }

    return '';
  }
}
