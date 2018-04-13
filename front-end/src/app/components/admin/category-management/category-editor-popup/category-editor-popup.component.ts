import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { Category } from '../../../../models/category.model';
import { CategoryService } from '../../../../services/category.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';

@Component({
    selector: 'category-editor-popup',
    templateUrl: './category-editor-popup.component.html'
})

export class CategoryEditorPopupComponent extends DialogComponent<any, any> {

    public category: Category;
    private errorMessage: string = '';

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private categoryService: CategoryService) {
        super(dialogService);
    }

    private onSave(): void {
        this.categoryService.saveCategory(this.category).subscribe((res) => {
            if (res) {
                this.category.id = res;
                this.result = this.category;
                this.close();
            } else {
                this.errorMessage = 'Lưu không thành công';
            }
        });
    }
}