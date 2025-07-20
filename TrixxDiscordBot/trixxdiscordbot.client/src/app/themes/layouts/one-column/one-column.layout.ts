import { Component, OnInit, ViewChild } from '@angular/core';
import { NbSidebarComponent, NbSidebarService } from '@nebular/theme';
import { Observable, of } from 'rxjs';
import { filter, map, startWith } from 'rxjs/operators';
import { RIGHT_SIDEBAR_TAG } from './consts';

@Component({
  standalone: false,
  selector: 'app-one-column-layout',
  styleUrls: ['./one-column.layout.scss'],
  template: `
    <nb-layout windowMode>
      <nb-layout-header fixed>
        <app-trixx-header></app-trixx-header>
      </nb-layout-header>

      <nb-sidebar class="menu-sidebar" tag="menu-sidebar" responsive>
        <ng-content select="nb-menu"></ng-content>
      </nb-sidebar>

      <nb-sidebar
        #rSideBar
        [tag]="rSidebarTag"
        [right]="true"
        [state]="'collapsed'"
        class="mpk-right-sidebar"
      >
        <app-right-sidebar-content-wrap
          [expanded$]="rightSideBarExpanded$"
        ></app-right-sidebar-content-wrap>
      </nb-sidebar>

      <nb-layout-column>
        <ng-content select="router-outlet"></ng-content>
      </nb-layout-column>
    </nb-layout>
  `,
})
export class OneColumnLayoutComponent implements OnInit {
  constructor(private readonly service: NbSidebarService) {}
  @ViewChild('rSideBar') public rightSideBarElement: NbSidebarComponent | null = null;

  public rSidebarTag = RIGHT_SIDEBAR_TAG;
  public rightSideBarExpanded$: Observable<boolean> = of(false);

  ngOnInit(): void {
    this.rightSideBarExpanded$ = this.service.onToggle().pipe(
      filter((x) => x.tag === this.rSidebarTag),
      map(() => this.rightSideBarElement!.expanded),
      startWith(false),
    );
  }
}
