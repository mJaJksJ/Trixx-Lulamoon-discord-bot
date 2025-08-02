import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { StudiosService } from '../../../../api/services';
import { StudiosListSelectItem } from '../../../../api/models';
import { TrixxTableColumn, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryStudiosEditComponent } from '../edit/dictionary-studios-edit.component';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-dictionary-studios-list',
  standalone: false,
  templateUrl: './dictionary-studios-list.component.html',
  styleUrl: './dictionary-studios-list.component.scss'
})
export class DictionaryStudiosListComponent implements AfterViewInit {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<StudiosListSelectItem>;
  private destroy$ = new Subject<void>();
  
  private readonly columns: TrixxTableColumn<StudiosListSelectItem>[] = [
    { key: 'id', name: 'Id' },
    { key: 'label', name: 'Название' },
  ];

  constructor(
    private readonly apiService: StudiosService,
    private readonly dialogService: NbDialogService,
  ) {
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<StudiosListSelectItem>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiStudiosGet();
    this.table.columns = this.columns;
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
}
