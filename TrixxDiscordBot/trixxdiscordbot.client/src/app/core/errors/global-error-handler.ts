import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, ErrorHandler } from '@angular/core';
import { INoErrorNotification } from '../interceptors/token-refresher.interceptor';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  private defaultErrorMessage = 'Произошла непредвиденная ошибка';

  constructor(private notificationService: NotificationService) {}

  public async handleError(error: Error | HttpErrorResponse): Promise<void> {
    this.logError(error);

    const message = await this.appErrorHandle(error);
    if (message) {
      this.notificationService.showDanger(
        message.message || this.defaultErrorMessage,
        message.title || 'Ошибка'
      );
    }
  }

  private async appErrorHandle(
    errorResponse: Error | HttpErrorResponse
  ): Promise<{ message?: string; title?: string } | null> {
    if (!navigator.onLine) {
      return { message: 'Нет подключения к сети' };
    }

    if (errorResponse instanceof HttpErrorResponse) {
      return await this.getHttpErrorResponseMessage(errorResponse);
    } else {
      const rejection = (errorResponse as any).rejection;
      if (rejection && rejection instanceof HttpErrorResponse) {
        return await this.getHttpErrorResponseMessage(rejection);
      } else {
        return { message: errorResponse.message };
      }
    }
  }

  private async getHttpErrorResponseMessage(
    errorResponse: HttpErrorResponse
  ): Promise<{ message?: string; title?: string } | null> {
    const doNotShowNotification =
      errorResponse as unknown as INoErrorNotification;
    if (doNotShowNotification.noNotification) {
      return null;
    }
    const objResult = await tryExtractObjectResult(errorResponse);
    return { message: objResult?.message, title: objResult?.title };
  }

  private logError(error: Error | HttpErrorResponse): void {
    console.group('*** ERROR ***');
    console.error(error);
    console.groupEnd();
  }
}

async function tryExtractObjectResult(errorResponse: HttpErrorResponse) {
  const { error } = errorResponse;
  if (!error) {
    return null;
  }
  try {
    const parsedError =
      error instanceof Blob
        ? JSON.parse(await error.text())
        : typeof error === 'object'
        ? error
        : JSON.parse(error);
    return parsedError;
  } catch {
    return null;
  }
}
