import { Component, ViewContainerRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Library } from '../../../../models/library.model';
import { LibraryService } from '../../../../services/library.service';
import { ToastComponent } from '../../../common/toast-notification/toast-notification.component';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'library-editor-popup',
    templateUrl: './library-editor-popup.component.html'
})

export class LibraryEditorPopupComponent extends DialogComponent<any, any> {

    public library: Library;
    private errorMessage: string = '';
    private toastComponent: ToastComponent;
    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private libraryService: LibraryService,
        private toastr: ToastsManager,
        private vcr: ViewContainerRef) {
        super(dialogService);
        this.toastComponent = new ToastComponent(toastr, vcr);
    }

    private onSave(): void {
        this.libraryService.saveLibrary(this.library).subscribe((res) => {
            if (res) {
                const isNew = this.library.id === 0;
                this.library.id = res;
                this.result = this.library;

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
