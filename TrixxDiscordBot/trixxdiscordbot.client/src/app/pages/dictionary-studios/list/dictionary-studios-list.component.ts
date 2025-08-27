import { AfterViewInit, Component, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { StudiosService } from '../../../../api/services';
import { StudiosListSelectItem } from '../../../../api/models';
import { ActionButtonTypes, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryStudiosEditComponent } from '../edit/dictionary-studios-edit.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-dictionary-studios-list',
  standalone: false,
  templateUrl: './dictionary-studios-list.component.html',
  styleUrl: './dictionary-studios-list.component.scss'
})
export class DictionaryStudiosListComponent implements AfterViewInit, OnDestroy {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<StudiosListSelectItem, object>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly apiService: StudiosService,
    private readonly dialogService: NbDialogService,
  ) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<StudiosListSelectItem, object>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiStudiosGet();
    this.table.columns =  [
    { key: 'label', name: 'Название' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editStudio(id), type: ActionButtonTypes.Edit },
      { action: (id: number) => this.deleteStudio(id), type: ActionButtonTypes.Delete },
    ]
    this.table.init();
  }
  
  addStudio() {
    this.dialogService
      .open(DictionaryStudiosEditComponent, { closeOnBackdropClick: false })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  editStudio(id: number) {
    this.dialogService
      .open(DictionaryStudiosEditComponent, { 
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

  deleteStudio(id: number) {
    this.apiService
      .apiStudiosIdDelete({id: id})
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.table.reload();
      })
  }
}
