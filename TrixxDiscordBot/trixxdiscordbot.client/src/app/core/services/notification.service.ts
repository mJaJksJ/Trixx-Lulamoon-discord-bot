import { Injectable, NgZone } from '@angular/core';
import {
  NbComponentStatus,
  NbGlobalPhysicalPosition,
  NbIconConfig,
  NbToastrConfig,
  NbToastrService,
} from '@nebular/theme';

export type NbCommonIconConfig = string | NbIconConfig;

@Injectable()
export class NotificationService {
  private commonConfig: Partial<NbToastrConfig> = {
    position: NbGlobalPhysicalPosition.TOP_RIGHT,
    destroyByClick: true,
    limit: 10,
    duration: 10000,
  };

  constructor(private toastrService: NbToastrService, private ngZone: NgZone) {}

  public showDanger(
    message: string,
    title: string,
    icon: NbCommonIconConfig = 'alert-triangle-outline'
  ): void {
    this.ngZone.runOutsideAngular(() => {
      this.toastrService.danger(
        message,
        title,
        this.getComputedErrorConfig(icon)
      );
    });
  }

  private getComputedErrorConfig(
    icon?: NbCommonIconConfig
  ): Partial<NbToastrConfig> {
    return Object.assign(this.getComputedConfig('danger', icon), {
      preventDuplicates: true,
      duplicatesBehaviour: 'previous',
    } as Partial<NbToastrConfig>);
  }

  private getComputedConfig(
    status: NbComponentStatus,
    icon?: NbCommonIconConfig
  ): Partial<NbToastrConfig> {
    return { ...this.commonConfig, status, icon };
  }
}
