import { Component, OnInit } from '@angular/core';
import { NbButtonModule, NbCardModule, NbDialogModule, NbDialogRef } from '@nebular/theme';
import {
  ConfirmModalConfig,
  ConfirmModalMessageTemplate,
  CONFIRM_MODAL_DEF_CONFIG,
} from '../confirm-modal.config';
import { CommonModule } from '@angular/common';

@Component({
  imports: [
    CommonModule,
    NbCardModule,
    NbDialogModule,
    NbButtonModule,
  ],
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.scss'],
})
export class ConfirmModalComponent implements OnInit {
  public config: ConfirmModalConfig = CONFIRM_MODAL_DEF_CONFIG;
  public mode: 'tpl' | 'string' | null = null;
  messageTpl?: ConfirmModalMessageTemplate;
  messageString?: string;

  constructor(public readonly dialogRef: NbDialogRef<ConfirmModalComponent>) {}
  ngOnInit(): void {
    if (this.config) {
      switch (typeof this.config.message) {
        case 'object':
          this.mode = 'tpl';
          this.messageTpl = this.config.message;
          break;
        case 'string':
          this.mode = 'string';
          this.messageString = this.config.message;
          break;
        default:
          this.mode = null;
          break;
      }
    }
  }
}
