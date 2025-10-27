import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CartoonPackService } from '../../../../../api/services';
import { NbDialogRef } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../../shared/utils/trixx-loading-subject';
import { CartoonsPackLabelTypeEditModel } from '../../../../../api/models';

@Component({
  selector: 'app-label-type-edit',
  standalone: false,
  templateUrl: './label-type-edit.component.html',
  styleUrl: './label-type-edit.component.scss',
})
export class LabelTypeEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;
  @Input() cartoonPackId?: number;
  @Input() order?: number;

  private formConf: { [x in keyof CartoonsPackLabelTypeEditModel]: FormControl } = {
    id: new FormControl(),
    name: new FormControl('', [Validators.required]),
  };
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  constructor(
    private readonly apiService: CartoonPackService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void { }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public get isEdit(): boolean {
    return !!this.id;
  }

  public save() {
    const data = this.form.getRawValue();
    data.cartoonPackId = this.cartoonPackId;
    data.order = this.order;
    
    this.apiService
      .apiCartoonPackEditLabelTypePost({ body: data })
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
