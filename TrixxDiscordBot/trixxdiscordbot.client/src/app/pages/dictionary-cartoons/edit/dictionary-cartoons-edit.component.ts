import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CartoonsService } from '../../../../api/services';
import { CartoonUpdateModel, SelectItem } from '../../../../api/models';
import { FormControl, Validators, FormGroup, FormArray, AbstractControl } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';
import { ArrayFieldConfig, TrixxFormArrayHelperComponent } from '../../../shared/modules/trixx-form-array-helper/trixx-form-array-helper.component';

@Component({
  selector: 'app-dictionary-cartoons-edit',
  standalone: false,
  templateUrl: './dictionary-cartoons-edit.component.html',
  styleUrl: './dictionary-cartoons-edit.component.scss'
})
export class DictionaryCartoonsEditComponent implements OnInit, OnDestroy {
  @ViewChild('alternativeNames', { read: TrixxFormArrayHelperComponent }) 
  public alternativeNamesComponent!: TrixxFormArrayHelperComponent;

  @ViewChild('studios', { read: TrixxFormArrayHelperComponent }) 
  public studiosComponent!: TrixxFormArrayHelperComponent;

  @ViewChild('sources', { read: TrixxFormArrayHelperComponent }) 
  public sourcesComponent!: TrixxFormArrayHelperComponent;

  @Input() public id?: number;

  private formConf: { [x in keyof CartoonUpdateModel]-?: AbstractControl } = {
    id: new FormControl(),
    name: new FormControl('', [Validators.required]),
    alternativeNames: new FormArray([]),
    sources: new FormArray([]),
    studios: new FormArray([]),
    year: new FormControl(),
  };
  public formArrayConfs: { [x in keyof CartoonUpdateModel]: ArrayFieldConfig[] } = {
    alternativeNames: [
      {
        type: 'text',
        name: 'value',
        label: 'Название',
        placeholder: 'Название',
        required: true
      }
    ],
    studios: [
      {
        type: 'select',
        name: 'value',
        label: 'Название',
        placeholder: 'Название',
        required: true,
        options$: new BehaviorSubject<SelectItem[]>([]),
      }
    ],
    sources: [
      {
        type: 'text',
        name: 'value',
        label: 'Название',
        placeholder: 'Название',
        required: true
      }
    ],
  }
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  constructor(
    private readonly apiService: CartoonsService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.apiService.apiCartoonsStudiosGet()
      .pipe(
        this.loading$.wrap(),
        takeUntil(this.destroy$),
      )
      .subscribe(studios => {
        this.formArrayConfs.studios?.forEach(x => x.options$?.next(studios))
      });

    if (this.id) {
      this.apiService
        .apiCartoonsIdGet({id: this.id})
        .pipe(
          takeUntil(this.destroy$),
        )
        .subscribe(cartoon => {
          this.form.patchValue({...cartoon, name: cartoon.label});
          this.form.markAsPristine();
          cartoon.alternativeNames?.forEach(x => this.alternativeNamesComponent.addItem({ value: x }));
          cartoon.studios?.forEach(x => this.studiosComponent.addItem({ value: x.id! }));
          cartoon.sources?.forEach(x => this.sourcesComponent.addItem({ value: x }));
        });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public get isEdit(): boolean {
    return !!this.id;
  }

  public save() {
    const data = this.form.getRawValue();
    data.alternativeNames = (data.alternativeNames as { value: string }[]).map(x => x.value);
    data.studios = (data.studios as { value: string }[]).map(x => x.value);
    data.sources = (data.sources as { value: string }[]).map(x => x.value);

    this.apiService
      .apiCartoonsPost({ body: data })
      .pipe(
        takeUntil(this.destroy$),
        this.loading$.wrap(),
      )
      .subscribe(() => this.dialogRef.close(true));
  }

  public handleClose() {
    this.dialogRef.close(false);
  }
}
