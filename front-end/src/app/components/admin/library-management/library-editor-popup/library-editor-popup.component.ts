import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Library } from '../../../../models/library.model';
import { LibraryService } from '../../../../services/library.service';

@Component({
    selector: 'library-editor-popup',
    templateUrl: './library-editor-popup.component.html'
})

export class LibraryEditorPopupComponent extends DialogComponent<any, any> {

    public library: Library;
    private errorMessage: string = '';

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private libraryService: LibraryService) {
        super(dialogService);
    }

    private onSave(): void {
        this.libraryService.saveLibrary(this.library).subscribe((res) => {
            if (res) {
                this.library.id = res;
                this.result = this.library;
                this.close();
            } else {
                this.errorMessage = 'Lưu không thành công';
            }
        });
    }
}
