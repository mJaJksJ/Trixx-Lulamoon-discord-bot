import { Component, Input } from '@angular/core';
import { Observable, shareReplay, Subject, switchMap } from 'rxjs';
import { TrixxLoadingSubject } from '../../utils/trixx-loading-subject';

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
export class TrixxTableComponent<T> {
  @Input() public apiGet!: () => Observable<T[]>;
  @Input() public columns!: TrixxTableColumn<T>[];
  @Input() public actions: IActionButton[] = [];
  public rows$!: Observable<T[]>;  
  private reloader$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();
  public readonly icons: Record<ActionButtonTypes, string> = {
    [ActionButtonTypes.Edit]: 'edit-outline',
    [ActionButtonTypes.Delete]: 'trash-2-outline',
  };

  init(): void {
    this.rows$ = this.reloader$.pipe(
      switchMap(() => {
        return this.apiGet().pipe(
          this.loading$.wrap(),
        );
      }),
      shareReplay(1), 
    );
    this.rows$.subscribe(); // TODO: поправить, в теории оно без этого субскрайба должно работать
    this.reloader$.next();
  }

  public reload() {
    this.reloader$.next();
  }
}
