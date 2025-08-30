import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CartoonsService } from '../../../../api/services';
import { CartoonFormDefaults, CartoonType, SelectItem } from '../../../../api/models';
import { FormControl, FormGroup, AbstractControl } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { Observable, Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';
import { cartoonTypesLabels } from '../utils';

@Component({
  selector: 'app-dictionary-cartoons-form-default',
  standalone: false,
  templateUrl: './dictionary-cartoons-form-default.component.html',
  styleUrl: './dictionary-cartoons-form-default.component.scss'
})
export class DictionaryCartoonsFormDefaultComponent implements OnInit, OnDestroy, AfterViewInit {
  private formConf: { [x in keyof CartoonFormDefaults]-?: AbstractControl } = {
    dictionaryStudioId: new FormControl(),
    cartoonType: new FormControl(),
  };
  public form = new FormGroup(this.formConf);
  public studios$!: Observable<SelectItem[]>;
  public cartoonTypes = Object.values(CartoonType).map(x => ({ id: x, label: cartoonTypesLabels[x] }));

  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  constructor(
    private readonly apiService: CartoonsService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.studios$ = this.apiService.apiCartoonsStudiosGet()
      .pipe(
        this.loading$.wrap(),
        takeUntil(this.destroy$),
      );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    this.cdr.detectChanges();

    this.apiService
      .apiCartoonsFormDefaultsGet()
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe(formDefault => {
        this.form.patchValue({...formDefault});
        this.form.markAsPristine();
      });
  }

  public save() {
    const data = this.form.getRawValue();

    this.apiService
      .apiCartoonsSetFormDefaultsPost({ body: data })
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
