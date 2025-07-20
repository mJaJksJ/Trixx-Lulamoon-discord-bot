import { NbComponentStatus } from '@nebular/theme';
import { TemplateRef } from '@angular/core';
import { ConfirmModalUtils } from './confirm-modal-utils';

export type ConfirmModalButtonsConfig = {
  content: string;
  returnVal: any;
  status?: NbComponentStatus;
}[];

export interface ConfirmModalMessageTemplate {
  readonly tpl: TemplateRef<any>;
  readonly context: any;
}

export interface ConfirmModalConfig {
  readonly title: string;
  readonly message?: string | ConfirmModalMessageTemplate;
  readonly btns: ConfirmModalButtonsConfig;
}

export const CONFIRM_MODAL_DEF_CONFIG: ConfirmModalConfig = {
  title: 'Подтверждение действия',
  message: 'Вы уверены что хотите сделать это',
  btns: ConfirmModalUtils.yesNoButtons({ yes: 'Да', no: 'Нет' }),
};
