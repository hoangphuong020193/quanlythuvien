import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../services/admin.service';
import { PagedList } from '../../../../models/paged-list.model';
import { UserNotReturnBook } from '../../../../models/admin.model';
import { JQueryHelper } from '../../../../shareds/helpers/jquery.helper';
import * as moment from 'moment';
import { Format } from '../../../../shareds/constant/format.constant';

@Component({
    selector: 'user-debt-book',
    templateUrl: './user-debt-book.component.html'
})
export class UserDebtBookComponent implements OnInit {

    private pageCurrent: number = 1;
    private pageSize: number = 10;

    private listUserDebt: PagedList<UserNotReturnBook>;

    constructor(
        private adminService: AdminService) { }

    public ngOnInit(): void {
        this.getListDebt(1);
    }

    private getListDebt(page: number, pageSize: number = this.pageSize): void {
        JQueryHelper.showLoading();
        this.pageCurrent = page;
        this.pageSize = pageSize;
        this.adminService.getListUserNotReturnBook(this.pageCurrent, this.pageSize)
            .subscribe((res) => {
                if (res) {
                    this.listUserDebt = res;
                    JQueryHelper.hideLoading();
                }
            });
    }

    private parseDateToString(date: Date): string {
        return moment(date).format(Format.DateFormat);
    }

    private calculatorOverDays(date: Date): number {
        return moment().diff(moment(date), 'days');
    }
}
