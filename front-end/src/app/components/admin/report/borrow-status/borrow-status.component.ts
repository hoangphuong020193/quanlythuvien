import { BookStatus } from './../../../../shareds/enums/book-status.enum';
import { Format } from './../../../../shareds/constant/format.constant';
import { PagedList } from './../../../../models/paged-list.model';
import { JQueryHelper } from './../../../../shareds/helpers/jquery.helper';
import { DropDownData } from './../../../common/dropdown/dropdown.component';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminService } from '../../../../services/admin.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import * as moment from 'moment';
import { BorrowStatus } from '../../../../models';
import { KeyCode } from '../../../../shareds/enums/keycode.enum';

@Component({
    selector: 'borrow-status',
    templateUrl: './borrow-status.component.html'
})
export class BorrowStatusComponent implements OnInit {

    private startDate: moment.Moment = moment(moment().format('YYYY-MM-01'));
    private endDate: moment.Moment = moment().endOf('month');
    private listDatas: PagedList<BorrowStatus>;

    private listLibraries: DropDownData[] = [];
    private listStatus: DropDownData[] = [];

    private pageCurrent: number = 1;
    private pageSize: number = 10;
    private selectedLibraryId: number = -3;
    private selectedStatus: number = -3;

    @ViewChild('searchInput') private searchInput: any;

    constructor(
        private store: Store<fromRoot.State>,
        private adminService: AdminService) { }

    public ngOnInit(): void {
        this.initData();
        this.getBorrowStatus(1);
    }

    private initData(): void {
        this.listStatus.push(new DropDownData(-3, 'Tất cả'));
        this.listStatus.push(new DropDownData(BookStatus.Pending, 'Chờ nhận'));
        this.listStatus.push(new DropDownData(BookStatus.Borrowing, 'Đang mượn'));
        this.listStatus.push(new DropDownData(BookStatus.OutDeadline, 'Hết hạn mượn'));
        this.listStatus.push(new DropDownData(BookStatus.Returned, 'Đã trả'));
        this.listStatus.push(new DropDownData(BookStatus.Cancel, 'Huỷ'));

        this.store.select(fromRoot.getLibrary).subscribe((res) => {
            this.listLibraries = [];
            if (res) {
                res.forEach((x) => {
                    if (x.enabled) {
                        this.listLibraries.push(new DropDownData(x.id, x.name));
                    }
                });
                if (this.listLibraries.length > 0) {
                    this.listLibraries.unshift(new DropDownData(-3, 'Tất cả'));
                }
            }
        });
    }

    private selectStartDate(date: moment.Moment): void {
        this.startDate = moment(date);
        this.getBorrowStatus(1);
    }

    private selectEndDate(date: moment.Moment): void {
        this.endDate = moment(date);
        this.getBorrowStatus(1);
    }

    private getBorrowStatus(page: number, pageSize: number = this.pageSize): void {
        JQueryHelper.showLoading();
        this.pageCurrent = page;
        this.pageSize = pageSize;

        const searchString: string = this.searchInput.nativeElement.value;

        this.adminService.getListBorrowStatus(
            this.pageCurrent,
            this.pageSize,
            this.startDate.format('DDMMYYYY'),
            this.endDate.format('DDMMYYYY'),
            this.selectedLibraryId,
            this.selectedStatus,
            searchString).subscribe((res) => {
                this.listDatas = res;
                JQueryHelper.hideLoading();
            });
    }

    private selectLibrary(data: DropDownData): void {
        this.selectedLibraryId = data.key;
        this.getBorrowStatus(1);
    }

    private selectStatus(data: DropDownData): void {
        this.selectedStatus = data.key;
        this.getBorrowStatus(1);
    }

    private parseDateToString(date: Date): string {
        if (!moment(date).isValid()) {
            return '-';
        }
        return moment(date).format(Format.DateFormat);
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

    private onKeyPress(event: any): void {
        if (event.keyCode === KeyCode.Enter || event.keyChar === KeyCode.Enter) {
            this.getBorrowStatus(1);
        }
    }
}
