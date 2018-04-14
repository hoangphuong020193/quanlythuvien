import { Component, ViewContainerRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { PublisherService } from '../../../../services/publisher.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Publisher } from '../../../../models/publisher.model';
import { ToastComponent } from '../../../common/toast-notification/toast-notification.component';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'publisher-editor-popup',
    templateUrl: './publisher-editor-popup.component.html'
})

export class PublisherEditorPopupComponent extends DialogComponent<any, any> {

    public publisher: Publisher;
    private errorMessage: string = '';

    private toastComponent: ToastComponent;

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private publisherService: PublisherService,
        private toastr: ToastsManager,
        private vcr: ViewContainerRef) {
        super(dialogService);
        this.toastComponent = new ToastComponent(toastr, vcr);
    }

    private onSave(): void {
        this.publisherService.savePublisher(this.publisher).subscribe((res) => {
            if (res) {
                const isNew = this.publisher.id === 0;
                this.publisher.id = res;
                this.result = this.publisher;
                if (isNew) {
                    this.toastComponent.showSuccess('Đã thêm thành công');
                } else {
                    this.toastComponent.showSuccess('Đã sửa thành công');
                }
                this.close();
            } else {
                this.errorMessage = 'Lưu không thành công';
            }
        });
    }
}
