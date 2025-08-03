import { ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  StudioUpdateModel,
} from '../../../../api/models';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StudiosService } from '../../../../api/services';
import { NbDialogRef } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';

@Component({
  selector: 'app-dictionary-studios-edit',
  standalone: false,
  templateUrl: './dictionary-studios-edit.component.html',
  styleUrl: './dictionary-studios-edit.component.scss',
})
export class DictionaryStudiosEditComponent implements OnDestroy, OnInit {
  @Input() public id?: number;

  private formConf: { [x in keyof StudioUpdateModel]-?: FormControl } = {
    id: new FormControl(),
    name: new FormControl('', [Validators.required]),
  };
  public form = new FormGroup(this.formConf);
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();

  constructor(
    private readonly apiService: StudiosService,
    public readonly dialogRef: NbDialogRef<any>,
    public readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    if (this.id) {
      this.apiService
        .apiStudiosIdGet({id: this.id})
        .pipe(
          takeUntil(this.destroy$),
        )
        .subscribe(studio => {
          this.form.patchValue({...studio, name: studio.label});
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
      .apiStudiosPost({ body: data })
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
