import { AdminService } from './../../../../services/admin.service';
import { DropDownData } from './../../../common/dropdown/dropdown.component';
import { Library } from './../../../../models/library.model';
import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import * as fromRoot from '../../../../store/reducers';
import { Store } from '@ngrx/store';
import { PagedList, ReadStatistic } from '../../../../models';
import { JQueryHelper } from '../../../../shareds/helpers/jquery.helper';

@Component({
    selector: 'read-statistics',
    templateUrl: './read-statistics.component.html'
})
export class ReadStatisticsComponent implements OnInit {

    private listGroupBy: DropDownData[] = [];
    private listDatas: PagedList<ReadStatistic>;
    private GroupBy: any = GroupBy;

    private startDate: moment.Moment = moment(moment().format('YYYY-MM-01'));
    private endDate: moment.Moment = moment().endOf('month');
    private selectedGroupById: number = 0;

    private pageCurrent: number = 1;
    private pageSize: number = 10;

    constructor(
        private store: Store<fromRoot.State>,
        private adminService: AdminService) { }

    public ngOnInit(): void {
        this.initData();
        this.getData(1);
    }

    private initData(): void {
        this.listGroupBy.push(new DropDownData(GroupBy.Book, 'Theo tên sách'));
        this.listGroupBy.push(new DropDownData(GroupBy.Category, 'Theo danh mục'));
        this.listGroupBy.push(new DropDownData(GroupBy.User, 'Theo bạn đọc'));
        this.listGroupBy.push(new DropDownData(GroupBy.Library, 'Theo chi nhánh'));
    }

    private selectStartDate(date: moment.Moment): void {
        this.startDate = moment(date);
        this.getData(1);
    }

    private selectEndDate(date: moment.Moment): void {
        this.endDate = moment(date);
        this.getData(1);
    }

    private selectGroupBy(data: DropDownData): void {
        this.selectedGroupById = data.key;
        this.getData(1);
    }

    private getData(page: number, pageSize: number = this.pageSize): void {
        JQueryHelper.showLoading();
        this.pageCurrent = page;
        this.pageSize = pageSize;

        this.adminService.getListReadStatistic(
            this.pageCurrent,
            this.pageSize,
            this.startDate.format('DDMMYYYY'),
            this.endDate.format('DDMMYYYY'),
            this.selectedGroupById).subscribe((res) => {
                this.listDatas = res;
                JQueryHelper.hideLoading();
            });
    }
}

export enum GroupBy {
    Book = 0,
    Category = 1,
    User = 2,
    Library = 3
}
