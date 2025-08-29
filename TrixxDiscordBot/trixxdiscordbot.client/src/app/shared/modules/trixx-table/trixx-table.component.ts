import { Component, Input } from '@angular/core';
import { combineLatest, Observable, of, shareReplay, Subject, switchMap } from 'rxjs';
import { TrixxLoadingSubject } from '../../utils/trixx-loading-subject';
import { FormGroup } from '@angular/forms';

export interface TrixxTableColumn<T> {
  key: keyof T;
  name: string;
}

export interface IActionButton {
  action: (id: number) => void;
  type: ActionButtonTypes;
}

export enum ActionButtonTypes {
    Edit = 'Редактировать',
    Delete = 'Удалить',
}

@Component({
  selector: 'app-trixx-table',
  standalone: false,
  templateUrl: './trixx-table.component.html',
  styleUrl: './trixx-table.component.scss'
})
export class TrixxTableComponent<T, TF> {
  @Input() public apiGet!: (filterModel?: TF) => Observable<T[]>;
  @Input() public columns!: TrixxTableColumn<T>[];
  @Input() public actions: IActionButton[] = [];
  @Input() public form: FormGroup | undefined = new FormGroup({});

  public rows$!: Observable<T[]>;  
  private reloader$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();
  public readonly icons: Record<ActionButtonTypes, string> = {
    [ActionButtonTypes.Edit]: 'edit-outline',
    [ActionButtonTypes.Delete]: 'trash-2-outline',
  };

  init(): void {
    this.rows$ = combineLatest({ reloader: this.reloader$, valueChanges: this.form?.valueChanges || of({}) }).pipe(
      switchMap((x) => {
        return this.apiGet(x.valueChanges).pipe(
          this.loading$.wrap(),
        );
      }),
      shareReplay(1), 
    );
    this.rows$.subscribe(); // TODO: поправить, в теории оно без этого субскрайба должно работать
    this.reloader$.next();
    this.form?.setValue(this.form.getRawValue());
  }

  public reload() {
    this.reloader$.next();
  }

  getArray(row: any, key: keyof T) {
    const val = row[key];
    return Array.isArray(val) ? val : [];
  }

  isArray(row: any, key: keyof T) {
    const val = row[key];
    return Array.isArray(val);
  }

  isRef(value: any) {
    return value && !!value.ref;
  }
}
