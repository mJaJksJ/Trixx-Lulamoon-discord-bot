import { AfterViewInit, Component, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { UserListItem } from '../../../../../api/models';
import { ActionButtonTypes, TrixxTableComponent } from '../../../../shared/modules/trixx-table/trixx-table.component';
import { UsersService } from '../../../../../api/services';
import { DictionaryUsersEditComponent } from '../edit/users-edit.component';

@Component({
  selector: 'app-users-list',
  standalone: false,
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.scss'
})
export class UsersListComponent implements AfterViewInit, OnDestroy {
  @ViewChild('table', { read: ViewContainerRef }) 
  public tableContainer!: ViewContainerRef;
  private table!: TrixxTableComponent<UserListItem, object>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly apiService: UsersService,
    private readonly dialogService: NbDialogService,
  ) {
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    const componentRef = this.tableContainer.createComponent(TrixxTableComponent<UserListItem, object>);
    this.table = componentRef.instance;
    this.table.apiGet = () => this.apiService.apiUsersUsersGet();
    this.table.columns =  [
    { key: 'label', name: 'Название' },
    ];
    this.table.actions = [
      { action: (id: number) => this.editUser(id), type: ActionButtonTypes.Edit },
      { action: (id: number, params: { toLock: boolean }) => this.changeLockStatus(id, params), type: ActionButtonTypes.Lock },
    ]
    this.table.init();
  }
  
  addUser() {
    this.dialogService
      .open(DictionaryUsersEditComponent, { closeOnBackdropClick: false })
      .onClose
      .pipe(takeUntil(this.destroy$))
      .subscribe((reload) => {
        if (reload) {
          this.table.reload();
        }
      });
  }

  editUser(id: number) {
    this.dialogService
      .open(DictionaryUsersEditComponent, { 
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

  changeLockStatus(id: number, params: { toLock: boolean }) {
    this.apiService
      .apiUsersChangeUserLockStatusUserIdPost({ userId: id, toLock: params.toLock })
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        this.table.reload();
      })
  }
}
