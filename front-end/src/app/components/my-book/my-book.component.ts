import { Component, OnInit } from '@angular/core';
import { MyBookService } from '../../services/my-book.service';
import { BookStatus } from '../../shareds/enums/book-status.enum';
import { MyBook } from '../../models/my-book.model';
import * as moment from 'moment';
import { Format } from '../../shareds/constant/format.constant';
import { PagedList } from '../../models/paged-list.model';
import { StorageKey } from '../../shareds/constant/storage-key.constant';
import { RouterService } from '../../services/router.service';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';

@Component({
    selector: 'my-book',
    templateUrl: './my-book.component.html'
})

export class MyBookComponent implements OnInit {
    private listStatus: number[] = [BookStatus.Borrowing
        , BookStatus.Pending, BookStatus.OutDeadline];
    private listMyBook: PagedList<MyBook>;

    private currentPage: number = 1;
    private defaultItemsPerPage: number = 10;
    private BookStatus: any = BookStatus;

    constructor(
        private myBookService: MyBookService,
        private route: RouterService
    ) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        const pageSize: string = localStorage.getItem(StorageKey.PageSize);
        if ($.isNumeric(pageSize)) {
            this.defaultItemsPerPage = parseInt(pageSize, 10);
        }

        this.getListMyBook(1);
    }

    private getListMyBook(page: number = 1, pageSize: number = this.defaultItemsPerPage): void {
        this.currentPage = page;
        this.defaultItemsPerPage = pageSize;
        this.myBookService.getListMyBook(this.listStatus.join(','), page, this.defaultItemsPerPage)
            .subscribe((res) => {
                this.listMyBook = res;
                JQueryHelper.hideLoading();
            });
    }

    private selectStatus(status: number, checked: boolean): void {
        JQueryHelper.showLoading();
        if (checked) {
            this.listStatus.push(status);
            if (this.listStatus.length === 4) {
                this.listStatus = [0];
            }
        } else {
            this.listStatus = this.listStatus.filter((x) => x !== status);
        }
        this.getListMyBook(1);
    }

    private parseDateToString(date: Date): string {
        if (date) {
            return moment(date).format(Format.DateFormat);
        }
        return '-';
    }

    private getStatus(status: number): string {
        switch (status) {
            case BookStatus.Pending:
                return 'Chờ nhận';
            case BookStatus.Borrowing:
                return 'Đang mượn';
            case BookStatus.OutDeadline:
                return 'Hết hạn mượn';
            case BookStatus.Returned:
                return 'Đã trả';
            case BookStatus.Cancel:
                return 'Huỷ';
        }
    }

    private navigateBookDetail(bookCode: string): void {
        this.route.bookDetail(bookCode);
    }
}
