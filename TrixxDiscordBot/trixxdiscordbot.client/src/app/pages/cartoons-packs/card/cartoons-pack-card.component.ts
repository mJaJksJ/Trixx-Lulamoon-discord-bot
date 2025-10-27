import { Component, OnDestroy, OnInit } from '@angular/core';
import { CartoonPackService } from '../../../../api/services';
import { BehaviorSubject, Subject, takeUntil, tap } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';
import { ActivatedRoute } from '@angular/router';
import { CartoonItem, CartoonsPackModel, LabelType } from '../../../../api/models';
import { NbDialogService } from '@nebular/theme';
import { LabelTypeEditComponent } from './edit-label-type/label-type-edit.component';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-cartoons-pack-card',
  standalone: false,
  templateUrl: './cartoons-pack-card.component.html',
  styleUrl: './cartoons-pack-card.component.scss',
})
export class CartoonsPacksCardComponent implements OnDestroy, OnInit {
  public id: number;
  private destroy$ = new Subject<void>();
  public readonly loading$ = new TrixxLoadingSubject();
  public name$ = new BehaviorSubject<string>('');
  public labelTypes$ = new BehaviorSubject<LabelType[]>([]);
  public reload$ = new Subject();

  constructor(
    private readonly apiService: CartoonPackService,
    private readonly dialogService: NbDialogService,
    public readonly route: ActivatedRoute,
  ) {
    this.id = route.snapshot.params['id'];
  }

  ngOnInit(): void {
    this.reload$.pipe(
      takeUntil(this.destroy$),
      tap(() => {
        this.apiService
          .apiCartoonPackCardIdGet({id: this.id})
          .pipe(
            takeUntil(this.destroy$),
            this.loading$.wrap(),
          )
          .subscribe((pack: CartoonsPackModel) => {
            this.name$.next(pack.name);
            pack.labelTypes.forEach(lt => lt.cartoons = [...lt.cartoons, { dictionaryCartoonId: -1 }]);
            this.labelTypes$.next(pack.labelTypes);
          });
      })
    ).subscribe();
    this.reload$.next(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getCartoonListId(order: number): string {
    return `${order + 1}`;
  }

  addLabelType() {
    this.dialogService
      .open(LabelTypeEditComponent, {
        closeOnBackdropClick: false,
        context: {
          cartoonPackId: this.id,
          order: this.labelTypes$.value.length - 1,
        },
       })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.reload$.next(null);
        }
      });
  }

  replaceCartoon(event: CdkDragDrop<CartoonItem[], CartoonItem[], CartoonItem>): void {
    console.log(event);
    if (!event.container.data || !event.previousContainer.data) {
      return;
    }

    if (event.previousContainer === event.container) {
      return;
    }

    transferArrayItem(
      event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      0,
    );

    const labelType = this.labelTypes$.value[+event.container.id];
    this.apiService
      .apiCartoonPackReplaceCartoonPackIdLabelTypeIdDictionaryCartoonIdPost({
        packId: this.id,
        labelTypeId: labelType.id,
        dictionaryCartoonId: event.item.data.dictionaryCartoonId,
      })
      .pipe(
        takeUntil(this.destroy$),
        this.loading$.wrap(),
      ).subscribe();
  }

  trackByOrder(index: number, item: LabelType) {
    return item.order;
  }

  trackByCartoon(index: number, cartoon: CartoonItem) {
    return cartoon.dictionaryCartoonId;
  }
}
