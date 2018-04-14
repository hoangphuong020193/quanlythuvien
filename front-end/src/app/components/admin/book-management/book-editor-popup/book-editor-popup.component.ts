import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { Book } from '../../../../models/book.model';
import { Category } from '../../../../models/category.model';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { DropDownData } from '../../../common/dropdown/dropdown.component';
import { getSupplier } from '../../../../store/reducers/index';
import { BookService } from '../../../../services/book.service';
import { Config } from '../../../../config';
import * as moment from 'moment';
import { Format } from '../../../../shareds/constant/format.constant';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { ToastComponent } from '../../../common/toast-notification/toast-notification.component';
import { PopupConfirmComponent } from '../../../common/popup-confirm/popup-confirm.component';

@Component({
    selector: 'book-editor-popup',
    templateUrl: './book-editor-popup.component.html'
})
export class BookEditorPopupComponent extends DialogComponent<any, any> implements OnInit {
    public book: Book;
    private toastComponent: ToastComponent;

    private categories: DropDownData[] = [];
    private publishers: DropDownData[] = [];
    private suppliers: DropDownData[] = [];
    private libraries: DropDownData[] = [];
    private errorMessage: string = '';

    @ViewChild('fileInput') private fileInput;

    private urlImg: string;
    private bookCodeExisted: boolean = false;
    private fileToUpload: any = null;

    private options: any = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ header: 1 }, { header: 2 }],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ size: ['small', false, 'large', 'huge'] }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],
            [{ color: [] }, { background: [] }],
            [{ align: [] }, 'link']
        ]
    };

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private bookService: BookService,
        private toastr: ToastsManager,
        private vcr: ViewContainerRef) {
        super(dialogService);
        this.toastComponent = new ToastComponent(toastr, vcr);
    }

    public ngOnInit() {
        this.store.select(fromRoot.getCategory).subscribe((res) => {
            this.categories = [];
            if (res) {
                res.forEach((x) => {
                    if (x.enabled) {
                        this.categories.push(new DropDownData(x.id, x.categoryName));
                    }
                });
            }
        });
        this.store.select(fromRoot.getPublisher).subscribe((res) => {
            this.publishers = [];
            if (res) {
                res.sort((a, b) => {
                    if (a.name < b.name) {
                        return -1;
                    }

                    if (a.name > b.name) {
                        return 1;
                    }
                    return 0;
                });
                res.forEach((x) => {
                    if (x.enabled) {
                        this.publishers.push(new DropDownData(x.id, x.name));
                    }
                });
            }
        });
        this.store.select(fromRoot.getSupplier).subscribe((res) => {
            this.suppliers = [];
            if (res) {
                res.sort((a, b) => {
                    if (a.name < b.name) {
                        return -1;
                    }

                    if (a.name > b.name) {
                        return 1;
                    }
                    return 0;
                });

                res.forEach((x) => {
                    if (x.enabled) {
                        this.suppliers.push(new DropDownData(x.id, x.name));
                    }
                });
            }
        });
        this.store.select(fromRoot.getLibrary).subscribe((res) => {
            this.libraries = [];
            if (res) {
                res.sort((a, b) => {
                    if (a.name < b.name) {
                        return -1;
                    }

                    if (a.name > b.name) {
                        return 1;
                    }
                    return 0;
                });
                res.forEach((x) => {
                    if (x.enabled) {
                        this.libraries.push(new DropDownData(x.id, x.name));
                    }
                });
            }
        });

        this.urlImg = Config.getBookImgApiUrl(this.book.bookCode);
    }

    private selectPublishDate(date: Date): void {
        this.book.publicationDate = moment(date).format(Format.DateFormatJson);
    }

    private bookCodeChange(bookCode: string): void {
        this.bookService.checkBookCodeExists(this.book.bookId, bookCode).subscribe((res) => {
            this.bookCodeExisted = res;
        });
    }

    private selectCategory(data: DropDownData): void {
        this.book.categoryId = data.key;
        this.book.categoryName = data.value;
    }

    private selectSupplier(data: DropDownData): void {
        this.book.supplierId = data.key;
        this.book.supplier = data.value;
    }

    private selectPublisher(data: DropDownData): void {
        this.book.publisherId = data.key;
        this.book.publisher = data.value;
    }

    private onSave(): void {
        if (!this.book.enabled) {
            this.dialogService.addDialog(PopupConfirmComponent, {
                title: 'Xác nhận ẩn sách khỏi hệ thống chọn sách',
                message: 'Bạn có muốn ẩn sách này khỏi hệ thống chọn sách?'
            }).subscribe((confirm) => {
                if (confirm) {
                    this.saveSach();
                }
            });
        } else {
            this.saveSach();
        }
    }

    private saveSach(): void {
        if (this.validationData()) {
            this.errorMessage = '';
            this.bookService.saveBook(this.book).subscribe((res) => {
                if (res) {
                    const isNew = this.book.bookId === 0;
                    this.book.bookId = res.bookId;
                    this.result = this.book;
                    if (this.fileToUpload !== null) {
                        this.bookService.saveImage(this.fileToUpload, this.book.bookId).subscribe();
                    }

                    if (!this.book.enabled) {
                        this.toastComponent.showSuccess('Đã ẩn sách khỏi hệ thống chọn sách');
                    }
                    else if (isNew) {
                        this.toastComponent.showSuccess('Đã thêm thành công sách');
                    } else {
                        this.toastComponent.showSuccess('Đã sửa thông tin sách');
                    }

                    this.close();
                }
            });
        } else {
            this.errorMessage = 'Vui lòng điền đầy đủ các trường bắt buộc';
        }
    }

    private onChooseFile(): void {
        $('.fileInput').click();
    }

    private selectedImg(fileInput: any): void {
        if (fileInput.target.files && fileInput.target.files[0]) {
            const reader = new FileReader();
            reader.onload = ((e) => {
                this.urlImg = e.target['result'];
            });
            reader.readAsDataURL(fileInput.target.files[0]);
            this.fileToUpload = fileInput.target.files[0];
        }
    }

    private validationData(): boolean {
        if (this.book.bookName === ''
            || this.book.bookCode === ''
            || this.book.categoryId === 0
            || this.book.supplierId === 0
            || this.book.publisherId === 0
            || this.book.libraryId === 0
            || !this.book.libraryId) {
            return false;
        }

        return true;
    }

    private selectLibrary(data: DropDownData): void {
        this.book.libraryId = data.key;
    }

    private bookAvaiable(enabled: boolean): void {
        this.book.enabled = enabled;
    }
}
