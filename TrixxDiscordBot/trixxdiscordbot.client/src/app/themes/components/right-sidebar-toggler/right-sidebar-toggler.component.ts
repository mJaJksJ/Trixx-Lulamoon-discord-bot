import { Component, OnInit } from '@angular/core';
import { NbSidebarService } from '@nebular/theme';
import { tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { RIGHT_SIDEBAR_TAG } from '../../layouts/one-column/consts';
import { RightSidebarManager } from '../../../core/services/right-sidebar-manager.service';

@Component({
  standalone: false,
  selector: 'app-right-sidebar-toggler',
  templateUrl: './right-sidebar-toggler.component.html',
  styleUrls: ['./right-sidebar-toggler.component.scss'],
})
export class RightSidebarTogglerComponent implements OnInit {
  private rSidebarTag = RIGHT_SIDEBAR_TAG;
  public isDisplay$: Observable<boolean> = of(false);

  constructor(
    private readonly sidebar: NbSidebarService,
    public readonly rSidebarManager: RightSidebarManager,
  ) {}

  ngOnInit(): void {
    this.isDisplay$ = this.rSidebarManager.isRegistered$.pipe(
      tap((vl) => {
        if (!vl) {
          this.sidebar.collapse(this.rSidebarTag);
        }
      }),
    );
  }

  public handleToggle(): void {
    this.sidebar.toggle(false, this.rSidebarTag);
  }
}
