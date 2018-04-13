import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { DialogService } from 'angularx-bootstrap-modal';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { JsHelper } from '../../../shareds/helpers/js.helper';
import { Library } from '../../../models/library.model';
import { LibraryEditorPopupComponent } from './library-editor-popup/library-editor-popup.component';
import { LibraryService } from '../../../services/library.service';

@Component({
    selector: 'library-management',
    templateUrl: './library-management.component.html'
})

export class LibraryManagementComponent implements OnInit {

    private listLibraries: Library[] = [];
    private listOrigins: Library[] = [];

    @ViewChild('inputBox') private inputBoxElement: any;

    constructor(
        private store: Store<fromRoot.State>,
        private libraryService: LibraryService,
        private dialogService: DialogService) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.store.select(fromRoot.getLibrary).subscribe((res) => {
            if (res) {
                this.listOrigins = res;
                this.listLibraries = this.listOrigins;
                JQueryHelper.hideLoading();
            }
        });
    }

    private onKeyUp(): void {
        const value: string = this.inputBoxElement.nativeElement.value;
        if (value !== '') {
            this.listLibraries =
                this.listOrigins.filter((x) =>
                    x.name.toLowerCase().search(value.toLowerCase()) !== -1);
        } else {
            this.listLibraries = this.listOrigins;
        }
    }

    private addNew(): void {
        this.libraryEditor(new Library());
    }

    private libraryEditor(library: Library, index: number = -1): void {
        this.dialogService.addDialog(LibraryEditorPopupComponent, {
            library: JsHelper.cloneObject(library)
        }).subscribe((res) => {
            if (res) {
                // TODO
            }
        });
    }

    private updateStatus(index: number, enabled: boolean): void {
        this.listLibraries[index].enabled = enabled;
        this.libraryService.saveLibrary(this.listLibraries[index]).subscribe();
    }
}
