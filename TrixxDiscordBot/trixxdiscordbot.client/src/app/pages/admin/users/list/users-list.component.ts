import { AfterViewInit, Component, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { Subject, takeUntil } from 'rxjs';
import { UserListItem } from '../../../../../api/models';
import { ActionButtonTypes, TrixxTableComponent } from '../../../../shared/modules/trixx-table/trixx-table.component';
import { UsersService } from '../../../../../api/services';
import { UsersEditComponent } from '../edit/users-edit.component';
import { ActivatedRoute } from '@angular/router';

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
    private readonly route: ActivatedRoute,
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

    const userId = +this.route.snapshot.queryParams['id'];
    if (userId) {
      this.editUser(userId);
    }
  }
  
  addUser() {
    this.dialogService
      .open(UsersEditComponent, { closeOnBackdropClick: false })
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
      .open(UsersEditComponent, { 
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
