import {
  ElementRef,
  Injectable,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import {
  ComponentPortal,
  ComponentType,
  DomPortal,
  TemplatePortal,
} from '@angular/cdk/portal';
import { BehaviorSubject, Observable, Unsubscribable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RightSidebarManager {
  public portalChange = new BehaviorSubject<
    | DomPortal<HTMLElement | ElementRef<HTMLElement>>
    | TemplatePortal<any>
    | ComponentPortal<any>
    | null
  >(null);
  public isRegistered$: Observable<boolean>;

  private viewContainerRef$ = new BehaviorSubject<ViewContainerRef | null>(null);
  private unsubscribable = {
    unsubscribe: () => {
      this.clear();
    },
  };

  constructor() {
    this.isRegistered$ = this.portalChange.pipe(map((vl) => !!vl));
  }

  public registerComponent<T = any>(component: ComponentType<T>) {
    this.portalChange.next(new ComponentPortal<T>(component));
    return this.unsubscribable;
  }

  public registerTemplate<T = any>(
    tplRef: TemplateRef<T>,
    context?: T,
  ): Unsubscribable {
    this.viewContainerRef$.subscribe((viewContainerRef) => {
      this.portalChange.next(
        new TemplatePortal<T>(tplRef, viewContainerRef!, context),
      );
    });
    return this.unsubscribable;
  }

  public registerDOMElement(
    elRef: HTMLElement | ElementRef<HTMLElement>,
  ): Unsubscribable {
    this.portalChange.next(new DomPortal(elRef));
    return this.unsubscribable;
  }

  public registerViewContainerRef(viewContainerRef: ViewContainerRef): void {
    this.viewContainerRef$.next(viewContainerRef);
  }

  public clear(): void {
    this.portalChange.next(null);
  }
}
