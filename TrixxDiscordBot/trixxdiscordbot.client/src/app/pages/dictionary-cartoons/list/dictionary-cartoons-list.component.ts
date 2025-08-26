import { AfterViewInit, Component, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { CartoonsListSelectItem } from '../../../../api/models';
import { CartoonsService } from '../../../../api/services';
import { ActionButtonTypes, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryCartoonsEditComponent } from '../edit/dictionary-cartoons-edit.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-dictionary-cartoons-list',
  standalone: false,
  templateUrl: './dictionary-cartoons-list.component.html',
  styleUrl: './dictionary-cartoons-list.component.scss'
})
export class DictionaryCartoonsListComponent  implements AfterViewInit, OnDestroy {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<CartoonsListSelectItem>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly apiService: CartoonsService,
    private readonly dialogService: NbDialogService,
  ) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<CartoonsListSelectItem>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiCartoonsGet();
    this.table.columns = [
      { key: 'label', name: 'Название' },
      { key: 'year', name: 'Год' },
      { key: 'studios', name: 'Студии' },
      { key: 'alternativeNames', name: 'Альтернативные названия' },
      { key: 'sources', name: 'Ссылки' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editCartoon(id), type: ActionButtonTypes.Edit },
      { action: (id: number) => this.deleteCartoon(id), type: ActionButtonTypes.Delete },
    ];
    componentRef.instance.init();
  }

addCartoon() {
    this.dialogService
      .open(DictionaryCartoonsEditComponent, { closeOnBackdropClick: false })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  editCartoon(id: number) {
    this.dialogService
      .open(DictionaryCartoonsEditComponent, { 
        closeOnBackdropClick: false,
        context: {
          id: id,
        }
      })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  deleteCartoon(id: number) {
    this.apiService
      .apiCartoonsIdDelete({id: id})
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.table.reload();
      })
  }
}
