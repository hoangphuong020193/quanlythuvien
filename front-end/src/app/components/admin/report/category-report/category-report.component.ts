import { JQueryHelper } from './../../../../shareds/helpers/jquery.helper';
import { AdminService } from './../../../../services/admin.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { KeyCode } from '../../../../shareds/enums/keycode.enum';
import { PagedList, CategoryReport } from '../../../../models';

@Component({
    selector: 'category-report',
    templateUrl: './category-report.component.html'
})
export class CategoryReportComponent implements OnInit {

    private listDatas: PagedList<CategoryReport>;
    private pageCurrent: number = 1;
    private pageSize: number = 10;

    @ViewChild('searchInput') private searchInput: any;

    constructor(private adminService: AdminService) { }

    public ngOnInit(): void {
        this.getCategoryReport(1);
    }

    private getCategoryReport(page: number, pageSize: number = this.pageSize): void {
        JQueryHelper.showLoading();
        this.pageCurrent = page;
        this.pageSize = pageSize;

        const searchString: string = this.searchInput.nativeElement.value;

        this.adminService.getCategoryReport(
            this.pageCurrent,
            this.pageSize,
            searchString
        ).subscribe((res) => {
            this.listDatas = res;
            JQueryHelper.hideLoading();
        });
    }

    private onKeyPress(event: any): void {
        if (event.keyCode === KeyCode.Enter || event.keyChar === KeyCode.Enter) {
            this.getCategoryReport(1);
        }
    }
}
