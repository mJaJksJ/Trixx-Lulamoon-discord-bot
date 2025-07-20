import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  NbMediaBreakpointsService,
  NbMenuService,
  NbSidebarService,
  NbThemeService,
  NbMenuItem,
} from '@nebular/theme';
import { map, Subject, takeUntil } from 'rxjs';
import { LayoutService } from '../../../core/utils/layout.service';
import { UserInfoService } from '../../../shared/services/user-info.service';

@Component({
  standalone: false,
  selector: 'app-trixx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class TrixxHeaderComponent implements OnInit, OnDestroy {
  private unsubs$: Subject<void> = new Subject<void>();
  userPictureOnly = false;

  userMenu: NbMenuItem[] = [
    { title: 'Выход', link: '/auth/logout', icon: 'log-out-outline' },
  ];
  public user: { name: string; } = { name: '' };
  public applicationInstanceName?: string;

  public constructor(
    private readonly sidebarService: NbSidebarService,
    private readonly menuService: NbMenuService,
    private readonly themeService: NbThemeService,
    private readonly layoutService: LayoutService,
    private readonly breakpointService: NbMediaBreakpointsService,
    private readonly userInfoService: UserInfoService,
  ) {}

  ngOnInit() {
    const { xl } = this.breakpointService.getBreakpointsMap();
    this.themeService
      .onMediaQueryChange()
      .pipe(
        map(([, currentBreakpoint]) => currentBreakpoint.width < xl),
        takeUntil(this.unsubs$),
      )
      .subscribe(
        (isLessThanXl: boolean) => (this.userPictureOnly = isLessThanXl),
      );
    
    this.userInfoService.data$
      .pipe(takeUntil(this.unsubs$))
      .subscribe((x) => {
        this.user = {
          name: x.name,
        };
      });
  }

  ngOnDestroy() {
    this.unsubs$.next();
    this.unsubs$.complete();
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }
}
