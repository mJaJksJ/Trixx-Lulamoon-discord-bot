import { AfterContentInit, ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { RefItem, SelectItem } from '../../../../api/models';

export interface ArrayFieldConfig {
  type: 'text'| 'select';
  name: string;
  label?: string;
  placeholder?: string;
  required?: boolean;
  defaultValue?: any;
  pattern?: string;
  options$?: BehaviorSubject<SelectItem[]>;
}

@Component({
  selector: 'app-trixx-form-array-helper',
  standalone: false,
  templateUrl: './trixx-form-array-helper.component.html',
  styleUrl: './trixx-form-array-helper.component.scss'
})
export class TrixxFormArrayHelperComponent implements AfterContentInit {
  @Input() formArray!: FormArray;
  @Input() fieldConfigs: ArrayFieldConfig[] = [];
  @Input() addButtonText = 'Добавить';
  @Input() removeButtonText = 'Удалить';
  @Input() minItems = 0;
  @Input() label!: string;

  @Output() itemAdded = new EventEmitter<void>();
  @Output() itemRemoved = new EventEmitter<number>();

  constructor(private readonly cdr: ChangeDetectorRef) {}

  ngAfterContentInit() {
    this.cdr.detectChanges();
  }

  addItem(value: Record<string, string | RefItem | number> | null = null): void {
    const newGroup = new FormGroup({});
    this.fieldConfigs.forEach(config => {
      const validators = [];
      if (config.required) {
        validators.push(Validators.required);
      }
      if (config.pattern) {
        validators.push(Validators.pattern(config.pattern));
      }

      let controlValue = value && value[config.name];
      if (typeof controlValue === 'object' && controlValue?.ref && controlValue.label) {
        controlValue = controlValue.label;
      }

      newGroup.addControl(
        config.name,
        new FormControl(controlValue || config.defaultValue || '', validators)
      );
    });

    this.formArray.push(newGroup);
    this.itemAdded.emit();
  }

  removeItem(index: number): void {
    this.formArray.removeAt(index);
    this.itemRemoved.emit(index);
  }

  trackByFn(index: number): number {
    return index;
  }

  get arrayGroups() {
    return this.formArray.controls.map(x => x as FormGroup);
  }
}
