import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { StorageKey } from '../../../shareds/constant/storage-key.constant';

@Component({
    selector: 'pagination',
    templateUrl: './pagination.component.html'
})
export class PaginationComponent implements OnInit {
    public static readonly DefaultItemsPerPage: number = 10;

    @Input() public totalCount: number = 0;
    @Input() public currentPageIndex: number = 0;
    @Input() public type: string;
    @Input() public itemsPerPage: number = PaginationComponent.DefaultItemsPerPage;

    @Output() public onPageChangeEvent: EventEmitter<number> = new EventEmitter();
    @Output() public onItemsPerPageChangeEvent: EventEmitter<number> = new EventEmitter();

    private toggle: boolean = false;

    public ngOnInit(): void {
        JQueryHelper.onClickOutside('.page-size', () => {
            this.toggle = false;
        });

        const pageSize: string = localStorage.getItem(StorageKey.PageSize);
        if ($.isNumeric(pageSize)) {
            this.itemsPerPage = parseInt(pageSize, 10);
        }
    }

    public onPageChange(page: number): void {
        this.onPageChangeEvent.emit(page);
    }

    private getCurrentShowing(): string {
        if (this.totalCount > 0) {
            return (this.itemsPerPage * (this.currentPageIndex - 1) + 1).toString() +
                '-' +
                (this.itemsPerPage * this.currentPageIndex > this.totalCount ?
                    this.totalCount ? this.totalCount : 0 :
                    this.itemsPerPage * this.currentPageIndex).toString();
        } else {
            return '0 - 0';
        }
    }

    private updateItemPerPage(itemsPerPage: number): void {
        this.itemsPerPage = itemsPerPage;
        this.onItemsPerPageChangeEvent.emit(itemsPerPage);
        this.toggle = false;
        localStorage.setItem(StorageKey.PageSize, itemsPerPage.toString());
    }
}
