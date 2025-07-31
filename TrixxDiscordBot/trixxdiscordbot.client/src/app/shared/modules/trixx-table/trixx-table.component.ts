import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs';

export interface TrixxTableColumn<T> {
  key: keyof T;
  name: string;
}

@Component({
  selector: 'app-trixx-table',
  standalone: false,
  templateUrl: './trixx-table.component.html',
  styleUrl: './trixx-table.component.scss'
})
export class TrixxTableComponent<T> {
  @Input() public apiGet!: () => Observable<T[]>;
  @Input() public columns!: TrixxTableColumn<T>[]
  public rows$!: Observable<T[]>;

  init(): void {
    this.rows$ = this.apiGet();
  }
}
