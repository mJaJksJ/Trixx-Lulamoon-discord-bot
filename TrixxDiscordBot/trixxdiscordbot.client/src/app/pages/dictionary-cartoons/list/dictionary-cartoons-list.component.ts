import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { CartoonsListSelectItem } from '../../../../api/models';
import { CartoonsService } from '../../../../api/services';
import { TrixxTableColumn, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { DictionaryCartoonsEditComponent } from '../edit/dictionary-cartoons-edit.component';

@Component({
  selector: 'app-dictionary-cartoons-list',
  standalone: false,
  templateUrl: './dictionary-cartoons-list.component.html',
  styleUrl: './dictionary-cartoons-list.component.scss'
})
export class DictionaryCartoonsListComponent  implements AfterViewInit {
  @ViewChild('table', { read: ViewContainerRef }) 
  public table!: ViewContainerRef;
  
  private readonly columns: TrixxTableColumn<CartoonsListSelectItem>[] = [
    { key: 'label', name: 'Название' },
    { key: 'year', name: 'Год' },
    { key: 'studios', name: 'Студии' },
    { key: 'alternativeNames', name: 'Альтернативные названия' },
    { key: 'sources', name: 'Ссылки' },
  ];

  constructor(
    private readonly apiService: CartoonsService,
    private readonly dialogService: NbDialogService,
  ) {
  }

  ngAfterViewInit(): void {
    const componentRef = this.table.createComponent(TrixxTableComponent<CartoonsListSelectItem>);
    componentRef.instance.apiGet = () => this.apiService.apiCartoonsGet();
    componentRef.instance.columns = this.columns;
    componentRef.instance.init();
  }

  addCartoon() {
    this.dialogService.open(DictionaryCartoonsEditComponent);
  }
}
