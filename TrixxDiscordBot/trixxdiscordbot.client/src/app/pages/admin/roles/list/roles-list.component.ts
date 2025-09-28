import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { RoleListItem } from '../../../../../api/models';
import { ActionButtonTypes, TrixxTableComponent } from '../../../../shared/modules/trixx-table/trixx-table.component';
import { RolesService } from '../../../../../api/services';
import { RolesEditComponent } from '../edit/roles-edit.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-roles-list',
  standalone: false,
  templateUrl: './roles-list.component.html',
  styleUrl: './roles-list.component.scss'
})
export class RolesListComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<RoleListItem, object>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly apiService: RolesService,
    private readonly dialogService: NbDialogService,
    private readonly route: ActivatedRoute,
  ) {
  }

  ngOnInit(): void {    
    const roleId = +this.route.snapshot.queryParams['id'];
    if (roleId) {
      this.editRole(roleId);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<RoleListItem, object>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiRolesRolesGet();
    this.table.columns =  [
    { key: 'label', name: 'Название' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editRole(id), type: ActionButtonTypes.Edit },
    ]
    this.table.init();
  }
  
  addRole() {
    this.dialogService
      .open(RolesEditComponent, { closeOnBackdropClick: false })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  editRole(id: number) {
    this.dialogService
      .open(RolesEditComponent, { 
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
