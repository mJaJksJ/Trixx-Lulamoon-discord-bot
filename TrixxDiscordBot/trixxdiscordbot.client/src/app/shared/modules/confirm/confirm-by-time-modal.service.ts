import { Injectable } from '@angular/core';
import { NbDialogRef, NbDialogService } from '@nebular/theme';
import { CONFIRM_MODAL_DEF_CONFIG, ConfirmModalConfig } from './confirm-modal.config';
import { ConfirmModalComponent } from './confirm-modal/confirm-modal.component';

@Injectable({
  providedIn: 'root',
})
export class ConfirmByTimeModalService {
  constructor(private readonly dialogService: NbDialogService) {}

  public async confirmWithAutoClose(
    config: Partial<ConfirmModalConfig>,
    autoCloseDurationSeconds: number,
  ): Promise<boolean | undefined> {
    const btnContent = (duration: number) => `Перейти на страницу входа (${duration})`;

    const confirmModalRef = this.dialogService.open(ConfirmModalComponent, {
      context: {
        config: {
          ...CONFIRM_MODAL_DEF_CONFIG,
          ...config,
          btns: [
            {
              content: btnContent(autoCloseDurationSeconds),
              returnVal: false,
              status: 'info',
            },
          ],
        },
      },
      hasBackdrop: false,
    });

    const tickFunc = (duration: number) => {
      if (duration !== 0) {
        setTimeout(() => {
          duration = duration - 1;
          confirmModalRef.componentRef.instance.config.btns[0].content = btnContent(duration);
          tickFunc(duration);
        }, 1000);
      } else {
        confirmModalRef.close();
      }
    };
    tickFunc(autoCloseDurationSeconds);
    return await this.returnConfirmOnClose(confirmModalRef);
  }

  private async returnConfirmOnClose<T>(confirmModalRef: NbDialogRef<T>) {
    const result = (await confirmModalRef.onClose.toPromise()) as boolean;
    return result;
  }
}
