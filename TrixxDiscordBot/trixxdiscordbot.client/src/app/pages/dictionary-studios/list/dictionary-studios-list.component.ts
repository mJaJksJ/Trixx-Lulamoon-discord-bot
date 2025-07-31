import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { StudiosService } from '../../../../api/services';
import { StudiosListSelectItem } from '../../../../api/models';
import { TrixxTableColumn, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryStudiosEditComponent } from '../edit/dictionary-studios-edit.component';

@Component({
  selector: 'app-dictionary-studios-list',
  standalone: false,
  templateUrl: './dictionary-studios-list.component.html',
  styleUrl: './dictionary-studios-list.component.scss'
})
export class DictionaryStudiosListComponent implements AfterViewInit {
  @ViewChild('table', { read: ViewContainerRef }) 
  public table!: ViewContainerRef;
  
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
    const componentRef = this.table.createComponent(TrixxTableComponent<StudiosListSelectItem>);
    componentRef.instance.apiGet = () => this.apiService.apiStudiosGet();
    componentRef.instance.columns = this.columns;
    componentRef.instance.init();
  }
  
  addStudio() {
    this.dialogService.open(DictionaryStudiosEditComponent);
  }
}
