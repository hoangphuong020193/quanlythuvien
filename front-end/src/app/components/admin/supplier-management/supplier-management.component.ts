import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { Supplier } from '../../../models/index';
import { DialogService } from 'angularx-bootstrap-modal';
import {
    SupplierEditorPopupComponent
} from './supplier-editor-popup/supplier-editor-popup.component';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { JsHelper } from '../../../shareds/helpers/js.helper';
import { SupplierService } from '../../../services/supplier.service';

@Component({
    selector: 'supplier-management',
    templateUrl: './supplier-management.component.html'
})

export class SupplierManagementComponent implements OnInit {

    private listSuppliers: Supplier[] = [];
    private listOrigins: Supplier[] = [];

    @ViewChild('inputBox') private inputBoxElement: any;

    constructor(
        private store: Store<fromRoot.State>,
        private supplierService: SupplierService,
        private dialogService: DialogService) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.store.select(fromRoot.getSupplier).subscribe((res) => {
            if (res) {
                this.listOrigins = res;
                this.listSuppliers = this.listOrigins;
                JQueryHelper.hideLoading();
            }
        });
    }

    private onKeyUp(): void {
        const value: string = this.inputBoxElement.nativeElement.value;
        if (value !== '') {
            this.listSuppliers =
                this.listOrigins.filter((x) =>
                    x.name.toLowerCase().search(value.toLowerCase()) !== -1);
        } else {
            this.listSuppliers = this.listOrigins;
        }
    }

    private addNew(): void {
        this.supplierEditor(new Supplier());
    }

    private supplierEditor(supplier: Supplier, index: number = -1): void {
        this.dialogService.addDialog(SupplierEditorPopupComponent, {
            supplier: JsHelper.cloneObject(supplier)
        }).subscribe((res) => {
            if (res) {
                // TODO
            }
        });
    }

    private updateStatus(index: number, enabled: boolean): void {
        this.listSuppliers[index].enabled = enabled;
        this.supplierService.saveSupplier(this.listSuppliers[index]).subscribe();
    }
}
