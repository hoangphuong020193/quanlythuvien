import { Component, ViewContainerRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { SupplierService } from '../../../../services/supplier.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Supplier } from '../../../../models/supplier.model';
import { ToastComponent } from '../../../common/toast-notification/toast-notification.component';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'supplier-editor-popup',
    templateUrl: './supplier-editor-popup.component.html'
})

export class SupplierEditorPopupComponent extends DialogComponent<any, any> {

    public supplier: Supplier;
    private errorMessage: string = '';
    private toastComponent: ToastComponent;
    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private supplierService: SupplierService,
        private toastr: ToastsManager,
        private vcr: ViewContainerRef) {
        super(dialogService);
        this.toastComponent = new ToastComponent(toastr, vcr);
    }

    private onSave(): void {
        this.supplierService.saveSupplier(this.supplier).subscribe((res) => {
            if (res) {
                const isNew = this.supplier.id === 0;
                this.supplier.id = res;
                this.result = this.supplier;

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
