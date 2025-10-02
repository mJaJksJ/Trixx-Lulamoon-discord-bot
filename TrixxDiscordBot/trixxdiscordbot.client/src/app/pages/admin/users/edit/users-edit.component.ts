import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../../shared/utils/trixx-loading-subject';
import { UsersService } from '../../../../../api/services';
import { SelectItem, UpdateUserModel, UserManuallyCreateModel, UserModel } from '../../../../../api/models';
import { ArrayFieldConfig, TrixxFormArrayHelperComponent } from '../../../../shared/modules/trixx-form-array-helper/trixx-form-array-helper.component';

@Component({
  selector: 'app-users-edit',
  standalone: false,
  templateUrl: './users-edit.component.html',
  styleUrl: './users-edit.component.scss',
})
export class UsersEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;

  private formConf: { [x in keyof (UserManuallyCreateModel & UpdateUserModel)]: AbstractControl } = {
    userName: new FormControl('', [Validators.required]),
    roles: new FormArray([]),
    password: new FormControl()
  };
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  @ViewChild('roles', { read: TrixxFormArrayHelperComponent }) 
  public rolesComponent!: TrixxFormArrayHelperComponent;

  public formArrayConfs: { [x in keyof UserModel]: ArrayFieldConfig[] } = {
    roles: [
      {
        type: 'select',
        name: 'value',
        label: 'Роль',
        placeholder: 'Роль',
        required: true,
        options$: new BehaviorSubject<SelectItem[]>([]),
        getLink: () => ['pages', 'roles'],
        getQueryParams: (x: number) => ({ id: x }),
      }
    ],
    userName: [],
  }

  constructor(
    private readonly apiService: UsersService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.apiService.apiUsersRolesPost()
      .pipe(
        this.loading$.wrap(),
        takeUntil(this.destroy$),
      )
      .subscribe(roles => {
        this.formArrayConfs.roles?.forEach(x => x.options$?.next(roles))
      });

    if (this.id) {
      this.apiService
        .apiUsersIdGet({ id: this.id })
        .pipe(
        takeUntil(this.destroy$),
        )
        .subscribe(user => {
          this.form.patchValue({ ...user });
          user.roles.forEach(x => this.rolesComponent.addItem({ value: x.id }));
          this.form.markAsPristine();
        });
      this.form.controls.userName?.disable();
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
    data.roles = (data.roles as { value: string }[]).map(x => x.value);
    if (this.id) {
      data.id = this.id;
    }

    (
      this.isEdit
      ? this.apiService
        .apiUsersUpdateUserPost({ body: data })
      : this.apiService
         .apiUsersCreateUserManuallyPost({ body: data })
    )
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
