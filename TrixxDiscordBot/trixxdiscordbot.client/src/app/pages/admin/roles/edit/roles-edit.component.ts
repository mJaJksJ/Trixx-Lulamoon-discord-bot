import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../../shared/utils/trixx-loading-subject';
import { RolesService } from '../../../../../api/services';
import { RoleEditModel } from '../../../../../api/models';

@Component({
  selector: 'app-roles-edit',
  standalone: false,
  templateUrl: './roles-edit.component.html',
  styleUrl: './roles-edit.component.scss',
})
export class RolesEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;

  private formConf: { [x in keyof RoleEditModel]-?: FormControl } = {
    id: new FormControl(),
    name: new FormControl('', [Validators.required]),
  };
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  constructor(
    private readonly apiService: RolesService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    if (this.id) {
      this.apiService
          .apiRolesIdGet({ id: this.id })
          .pipe(
          takeUntil(this.destroy$),
          )
          .subscribe(role => {
          this.form.patchValue({ ...role });
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
      .apiRolesEditRolePost({ body: data })
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
