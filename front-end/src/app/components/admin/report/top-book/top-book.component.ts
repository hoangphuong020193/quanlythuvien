import { Component, OnInit } from '@angular/core';
import { PagedList } from '../../../../models/paged-list.model';
import { BookBorrowAmount } from '../../../../models/book.model';
import { BookService } from '../../../../services/book.service';
import { JQueryHelper } from '../../../../shareds/helpers/jquery.helper';
import * as moment from 'moment';

@Component({
    selector: 'top-book',
    templateUrl: './top-book.component.html'
})
export class TopBookComponent implements OnInit {
    constructor(
        private bookService: BookService) { }

    private startDate: moment.Moment = moment(moment().format('YYYY-MM-01'));
    private endDate: moment.Moment = moment().endOf('month');

    private listBooks: PagedList<BookBorrowAmount>;
    private pageCurrent: number = 1;
    private pageSize: number = 10;

    public ngOnInit(): void {
        this.getTopBook(1);
    }

    private getTopBook(page: number, pageSize: number = this.pageSize): void {
        JQueryHelper.showLoading();
        this.pageCurrent = page;
        this.pageSize = pageSize;

        this.bookService.topBook(this.pageCurrent,
            this.pageSize,
            this.startDate.format('DDMMYYYY'),
            this.endDate.format('DDMMYYYY')
        )
            .subscribe((res) => {
                this.listBooks = res;
                JQueryHelper.hideLoading();
            });
    }

    private selectStartDate(date: moment.Moment): void {
        this.startDate = moment(date);
        this.getTopBook(1);
    }

    private selectEndDate(date: moment.Moment): void {
        this.endDate = moment(date);
        this.getTopBook(1);
    }
}
