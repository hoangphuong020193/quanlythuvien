import { ErrorHandler } from '@angular/core';

export class SystemErrorHandler implements ErrorHandler {
    public handleError(error: any): void {
        console.error('Error handled by system ', error);
    }
}