import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { CartoonsFilterModel, CartoonsListSelectItem, SelectItem } from '../../../../api/models';
import { CartoonsService } from '../../../../api/services';
import { ActionButtonTypes, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryCartoonsEditComponent } from '../edit/dictionary-cartoons-edit.component';
import { Observable, of, Subject, takeUntil } from 'rxjs';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { DictionaryCartoonsFormDefaultComponent } from '../form-default/dictionary-cartoons-form-default.component';

@Component({
  selector: 'app-dictionary-cartoons-list',
  standalone: false,
  templateUrl: './dictionary-cartoons-list.component.html',
  styleUrl: './dictionary-cartoons-list.component.scss'
})
export class DictionaryCartoonsListComponent  implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<CartoonsListSelectItem, CartoonsFilterModel>;
  private destroy$ = new Subject<void>();

  private formConf: { [x in keyof CartoonsFilterModel]-?: AbstractControl } = {
    search: new FormControl(''),
    studioId: new FormControl(),
  };
  public form = new FormGroup(this.formConf);
  public studios$: Observable<SelectItem[]> = of([]);

  constructor(
    private readonly apiService: CartoonsService,
    private readonly dialogService: NbDialogService,
  ) {
  }

  ngOnInit(): void {
    this.studios$ = this.apiService.apiCartoonsStudiosGet();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<CartoonsListSelectItem, CartoonsFilterModel>);
    this.table = componentRef.instance;
    this.table.apiGet = (filterModel?: CartoonsFilterModel) => this.apiService.apiCartoonsSearchPost({ body: filterModel });
    this.table.columns = [
      { key: 'label', name: 'Название' },
      { key: 'type', name: 'Тип' },
      { key: 'year', name: 'Год' },
      { key: 'studios', name: 'Студии' },
      { key: 'alternativeNames', name: 'Альтернативные названия' },
      { key: 'sources', name: 'Ссылки' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editCartoon(id), type: ActionButtonTypes.Edit },
      { action: (id: number) => this.deleteCartoon(id), type: ActionButtonTypes.Delete },
    ];
    this.table.form = this.form;
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

  setFormDefaults() {
    this.dialogService
      .open(DictionaryCartoonsFormDefaultComponent, { 
        closeOnBackdropClick: false,
      })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe();
  }
}
