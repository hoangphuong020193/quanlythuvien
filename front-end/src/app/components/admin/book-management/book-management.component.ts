import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { Format } from '../../../shareds/constant/format.constant';
import { BookService } from '../../../services/book.service';
import { Book } from '../../../models/book.model';
import { PagedList } from '../../../models/paged-list.model';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { KeyCode } from '../../../shareds/enums/keycode.enum';
import { DialogService } from 'angularx-bootstrap-modal';
import { BookEditorPopupComponent } from '../index';

@Component({
    selector: 'book-management',
    templateUrl: './book-management.component.html'
})
export class BookManagementComponent implements OnInit {
    private listBooks: PagedList<Book>;
    private currentPage: number = 1;
    private defaultItemsPerPage: number = 10;

    constructor(
        private bookService: BookService,
        private dialogService: DialogService) { }

    public ngOnInit() {
        this.getListBook(1);
    }

    private onKeyPress(event: any): void {
        if (event.keyCode === KeyCode.Enter || event.keyChar === KeyCode.Enter) {
            this.getListBook(1);
        }
    }

    private getListBook(pageIndex: number, pageSize: number = this.defaultItemsPerPage): void {
        JQueryHelper.showLoading();
        this.currentPage = pageIndex;
        this.defaultItemsPerPage = pageSize;
        const search: string = $('#search-book-box').val().toString();

        this.bookService.getListBook(search, this.currentPage, this.defaultItemsPerPage)
            .subscribe((res) => {
                this.listBooks = res;
                JQueryHelper.hideLoading();
            });
    }

    private parseDateToString(date: Date): string {
        if (date == null) {
            return '-';
        }
        return moment(date).format(Format.DateFormat);
    }

    private editBook(book: Book): void {
        this.dialogService.addDialog(BookEditorPopupComponent, {
            book
        }).subscribe((res) => {
            if (res) {
                this.getListBook(this.currentPage);
            }
        });
    }

    private addNewBook(): void {
        this.dialogService.addDialog(BookEditorPopupComponent, {
            book: new Book()
        }).subscribe((res) => {
            if (res) {
                this.getListBook(this.currentPage);
            }
        });
    }
}
