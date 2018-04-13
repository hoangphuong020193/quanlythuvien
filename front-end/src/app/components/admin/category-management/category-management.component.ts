import { Component, OnInit } from '@angular/core';
import { Category } from '../../../models/index';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { JsHelper } from '../../../shareds/helpers/js.helper';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { DialogService } from 'angularx-bootstrap-modal';
import { CategoryService } from '../../../services/category.service';
import {
    CategoryEditorPopupComponent
} from './category-editor-popup/category-editor-popup.component';

@Component({
    selector: 'category-management',
    templateUrl: './category-management.component.html'
})
export class CategoryManagementComponent implements OnInit {

    private listCategories: Category[] = [];
    private listOrigins: Category[] = [];

    constructor(
        private store: Store<fromRoot.State>,
        private categoryService: CategoryService,
        private dialogService: DialogService) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.store.select(fromRoot.getCategory).subscribe((res) => {
            if (res) {
                this.listOrigins = res;
                this.listCategories = this.listOrigins;
                JQueryHelper.hideLoading();
            }
        });
    }

    private addNew(): void {
        this.categoryEditor(new Category());
    }

    private categoryEditor(category: Category, index: number = -1): void {
        this.dialogService.addDialog(CategoryEditorPopupComponent, {
            category: JsHelper.cloneObject(category)
        }).subscribe((res) => {
            if (res) {
                // TODO
            }
        });
    }

    private updateStatus(index: number, enabled: boolean): void {
        this.listCategories[index].enabled = enabled;
        this.categoryService.saveCategory(this.listCategories[index]).subscribe();
    }
}