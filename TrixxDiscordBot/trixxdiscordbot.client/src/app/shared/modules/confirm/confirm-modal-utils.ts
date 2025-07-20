import { ConfirmModalButtonsConfig } from "./confirm-modal.config";

export class ConfirmModalUtils {
  public static yesNoButtons(conf: {
    yes: string;
    no: string;
  }): ConfirmModalButtonsConfig {
    return [
      {
        content: conf.no,
        returnVal: false,
        status: 'primary',
      },
      {
        content: conf.yes,
        returnVal: true,
        status: 'warning',
      },
    ];
  }
}
