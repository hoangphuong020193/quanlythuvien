import { Component, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { ToastOptions, ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
    selector: 'toast',
    encapsulation: ViewEncapsulation.None,
    template: ``
})

export class ToastComponent {
    constructor(private toastr: ToastsManager, vcr: ViewContainerRef) {
        this.toastr.setRootViewContainerRef(vcr);
    }

    private option: any = {
        toastLife: 2000,
        showCloseButton: true,
        positionClass: 'toast-bottom-right',
        animate: 'flyRight'
    };

    public showSuccess(message: string, title: string = 'Success!'): void {
        this.toastr.success(message, title, this.option);
    }

    public showError(message: string, title: string = 'Oops!'): void {
        this.toastr.error(message, title, this.option);
    }

    public showWarning(message: string, title: string = 'Alert!'): void {
        this.toastr.warning(message, this.option);
    }

    public showInfo(message: string, title: string = 'Info!'): void {
        this.toastr.info(message, this.option);
    }
}
