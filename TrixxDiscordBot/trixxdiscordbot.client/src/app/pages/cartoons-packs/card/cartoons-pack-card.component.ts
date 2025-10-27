import { Component, OnDestroy, OnInit } from '@angular/core';
import { CartoonPackService } from '../../../../api/services';
import { BehaviorSubject, debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { TrixxLoadingSubject } from '../../../shared/utils/trixx-loading-subject';
import { ActivatedRoute } from '@angular/router';
import { CartoonItem, CartoonsPackModel, CartoonType, LabelType } from '../../../../api/models';
import { NbDialogService } from '@nebular/theme';
import { LabelTypeEditComponent } from './edit-label-type/label-type-edit.component';
import { CdkDragDrop, transferArrayItem } from '@angular/cdk/drag-drop';
import { FormControl, FormGroup } from '@angular/forms';
import { comareStrings } from '../../../shared/utils/string-utils';

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
  public labelTypes: LabelType[] = [];
  public shownLabelTypes: LabelType[] = [];
  public reload$ = new Subject();
  public form = new FormGroup({
    search: new FormControl(''),
    cartoonType: new FormControl(null),
  });
  public cartoonTypes = Object.values(CartoonType).map(x => ({ id: x, label: x }));

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
            pack.labelTypes.forEach(lt => lt.cartoons = [...lt.cartoons, { dictionaryCartoonId: -1, alternativeNames: '', name: '' }]);
            this.labelTypes = pack.labelTypes;
            this.shownLabelTypes = pack.labelTypes.map(lt => ({...lt, cartoons: lt.cartoons.map(ltc => ({...ltc}))}));
          });
      })
    ).subscribe();
    this.reload$.next(null);
    this.form.valueChanges
      .pipe(
        debounceTime(300),
        takeUntil(this.destroy$),
      )
      .subscribe((x) => {
        const labelTypes = this.labelTypes.map(lt => ({...lt, cartoons: lt.cartoons.map(ltc => ({...ltc}))}));
        labelTypes.forEach(labelType => {
          if (x.search) {
            labelType.cartoons = labelType.cartoons.filter(c => c.dictionaryCartoonId === -1 || comareStrings(x.search!, [c.name, c.alternativeNames]))
          }
          if (x.cartoonType) {
            labelType.cartoons = labelType.cartoons.filter(c => c.dictionaryCartoonId === -1 || c.cartoonType === x.cartoonType)
          }
        });
        this.shownLabelTypes = labelTypes;
      });
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
          order: this.labelTypes.length - 1,
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
    if (!event.container.data || !event.previousContainer.data) {
      return;
    }

    if (event.previousContainer === event.container) {
      return;
    }

    const previousIndex = this.labelTypes[+event.previousContainer.id].cartoons
      .findIndex(x => x.dictionaryCartoonId === this.shownLabelTypes[+event.previousContainer.id].cartoons[event.previousIndex].dictionaryCartoonId);

    transferArrayItem(
      this.shownLabelTypes[+event.previousContainer.id].cartoons,
      this.shownLabelTypes[+event.container.id].cartoons,
      event.previousIndex,
      0,
    );

    transferArrayItem(
      this.labelTypes[+event.previousContainer.id].cartoons,
      this.labelTypes[+event.container.id].cartoons,
      previousIndex,
      0,
    );

    const labelType = this.labelTypes[+event.container.id];
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
