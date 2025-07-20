import { Component, OnInit } from '@angular/core';
import { MENU_ITEMS, TrixxMenuItem } from './pages-menu';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { TrixxAccessCheckerService } from '../core/services/trixx-access-checker.service';

@Component({
  standalone: false,
  selector: 'app-pages',
  styleUrls: ['pages.component.scss'],
  template: `
    <app-one-column-layout>
      <nb-menu [items]="(menu$ | async) || []"></nb-menu>
      <router-outlet></router-outlet>
    </app-one-column-layout>
  `,
})
export class PagesComponent implements OnInit {
  public menu$: Observable<TrixxMenuItem[]>;

  constructor(
    private readonly accessChecker: TrixxAccessCheckerService,
  ) {
    this.menu$ = accessChecker.changes$.pipe(
        map(() => this.filter(MENU_ITEMS())),
      );
  }

  ngOnInit(): void { }

  private filter(items: TrixxMenuItem[]): TrixxMenuItem[] {
    const result = [] as TrixxMenuItem[];
    items.forEach(originalItem => {
      if (originalItem.permission && !this.accessChecker.isGranted(originalItem.permission)) {
        return;
      }
      const item = { ...originalItem };
      if (item.children) {
        item.children = this.filter(item.children);
        if (!item.children.length && !item.link) {
          return;
        }
      }
      result.push(item);
    });

    return result;
  }
}
