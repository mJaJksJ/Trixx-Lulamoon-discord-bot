import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../../shared/utils/trixx-loading-subject';
import { RolesService } from '../../../../../api/services';
import { CommonPermission, Permission, RoleEditModel, SelectItem, WorkscreenPermissionsModel } from '../../../../../api/models';

@Component({
  selector: 'app-roles-edit',
  standalone: false,
  templateUrl: './roles-edit.component.html',
  styleUrl: './roles-edit.component.scss',
})
export class RolesEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;

  private formConf: { [x in keyof RoleEditModel]-?: FormControl | FormArray } = {
    id: new FormControl(),
    name: new FormControl('', [Validators.required]),
    permissions: new FormArray<FormControl<boolean>>([]),
  };
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();
  public workscreenHeaders: WorkscreenPermissionsModel[] = [];
  public commonPermissionHeaders = Object.values(CommonPermission);
  public usersUsage: SelectItem[] = [];
  public permissionsByOrder: Record<number, Permission> = {};

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
            this.form.controls['id'].setValue(this.id);

            this.workscreenHeaders = role.permissions;
            let idx = 0;
            const controls = role.permissions.flatMap(w =>
              w.permissions.map(p => {
                if (p.id) {
                  this.permissionsByOrder[idx] = p.id;
                }
                idx++;
                return new FormControl(p.isGranted);
              })
            );
            this.form.setControl('permissions', new FormArray(controls));

            this.usersUsage = role.usersUsage;

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
    const permissions = [];
    for (const [idx, p] of data.permissions.entries()) {
      if (p === true) {
        permissions.push(this.permissionsByOrder[idx]);
      }
    }
    data.permissions = permissions;

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

  get permissionsArray(): FormArray {
    return this.form.get('permissions') as FormArray;
  }
}
