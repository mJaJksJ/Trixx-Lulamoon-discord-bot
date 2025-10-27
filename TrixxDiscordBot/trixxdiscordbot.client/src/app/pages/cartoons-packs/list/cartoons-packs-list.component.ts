import { AfterViewInit, Component, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { CartoonPackService } from '../../../../api/services';
import { SelectItem } from '../../../../api/models';
import { ActionButtonTypes, TrixxTableComponent } from '../../../shared/modules/trixx-table/trixx-table.component';
import { NbDialogService } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { CartoonsPacksEditComponent } from '../edit/cartoons-pack-edit.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cartoons-packs-list',
  standalone: false,
  templateUrl: './cartoons-packs-list.component.html',
  styleUrl: './cartoons-packs-list.component.scss'
})
export class CartoonsPacksListComponent implements AfterViewInit, OnDestroy {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<SelectItem, object>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly apiService: CartoonPackService,
    private readonly dialogService: NbDialogService,
    private readonly router: Router,
  ) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<SelectItem, object>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiCartoonPackGet();
    this.table.columns =  [
    { key: 'label', name: 'Название' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editPack(id), type: ActionButtonTypes.Edit },
      { action: (id: number) => { this.router.navigate(['/pages/cartoons-packs/card', id]) }, type: ActionButtonTypes.OpenCard },
    ]
    this.table.init();
  }
  
  addPack() {
    this.dialogService
      .open(CartoonsPacksEditComponent, { closeOnBackdropClick: false })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  editPack(id: number) {
    this.dialogService
      .open(CartoonsPacksEditComponent, { 
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
}
