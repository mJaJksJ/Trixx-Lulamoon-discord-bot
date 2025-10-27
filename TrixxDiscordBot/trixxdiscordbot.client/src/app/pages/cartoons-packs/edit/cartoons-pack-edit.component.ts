import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  CartoonsPackEditModel,
} from '../../../../api/models';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CartoonPackService } from '../../../../api/services';
import { NbDialogRef } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';

@Component({
  selector: 'app-cartoons-pack-edit',
  standalone: false,
  templateUrl: './cartoons-pack-edit.component.html',
  styleUrl: './cartoons-pack-edit.component.scss',
})
export class CartoonsPacksEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;

  private formConf: { [x in keyof CartoonsPackEditModel]-?: FormControl } = {
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

  ngOnInit(): void {
    if (this.id) {
      this.apiService
        .apiCartoonPackIdGet({id: this.id})
        .pipe(
          takeUntil(this.destroy$),
        )
        .subscribe(pack => {
          this.form.patchValue({...pack, name: pack.label });
          this.form.markAsPristine();
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
    this.apiService
      .apiCartoonPackEditCartoonsPackPost({ body: data })
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
