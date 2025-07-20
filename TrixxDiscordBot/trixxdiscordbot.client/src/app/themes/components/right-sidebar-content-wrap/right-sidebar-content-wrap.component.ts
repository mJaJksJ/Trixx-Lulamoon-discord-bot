import { Portal } from '@angular/cdk/portal';
import { Component, Input, OnInit, ViewContainerRef } from '@angular/core';
import { Observable, of } from 'rxjs';
import { RightSidebarManager } from '../../../core/services/right-sidebar-manager.service';

@Component({
  standalone: false,
  selector: 'app-right-sidebar-content-wrap',
  templateUrl: './right-sidebar-content-wrap.component.html',
  styleUrls: ['./right-sidebar-content-wrap.component.scss'],
})
export class RightSidebarContentWrapComponent implements OnInit {
  portal$: Observable<Portal<any>>;

  @Input()
  public expanded$: Observable<boolean> = of(false);

  constructor(
    private readonly viewContainerRef: ViewContainerRef,
    private readonly rSidebarManager: RightSidebarManager,
  ) {
    this.portal$ = this.rSidebarManager.portalChange as any;
  }

  ngOnInit(): void {
    this.rSidebarManager.registerViewContainerRef(this.viewContainerRef);
  }
}
