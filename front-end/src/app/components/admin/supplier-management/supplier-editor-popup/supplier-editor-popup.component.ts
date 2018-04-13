import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { SupplierService } from '../../../../services/supplier.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Supplier } from '../../../../models/supplier.model';

@Component({
    selector: 'supplier-editor-popup',
    templateUrl: './supplier-editor-popup.component.html'
})

export class SupplierEditorPopupComponent extends DialogComponent<any, any> {

    public supplier: Supplier;
    private errorMessage: string = '';

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private supplierService: SupplierService) {
        super(dialogService);
    }

    private onSave(): void {
        this.supplierService.saveSupplier(this.supplier).subscribe((res) => {
            if (res) {
                this.supplier.id = res;
                this.result = this.supplier;
                this.close();
            } else {
                this.errorMessage = 'Lưu không thành công';
            }
        });
    }
}
