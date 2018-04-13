import { Component, OnInit } from '@angular/core';
import { SupplierService } from '../../services/supplier.service';
import { PublisherService } from '../../services/publisher.service';
import { CategoryService } from '../../services/category.service';
import { LibraryService } from '../../services/library.service';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {

    private borrowReturnBookTab: number = 1;
    private bookManagementTab: number = 2;
    private categoryTab: number = 3;
    private publisherManagementTab: number = 4;
    private supplierManagementTab: number = 5;
    private libraryManagementTab: number = 6;
    private reportTab: number = 7;
    private selectedTab: number = this.borrowReturnBookTab;

    constructor(
        private categoryService: CategoryService,
        private supplierService: SupplierService,
        private publisherService: PublisherService,
        private libraryService: LibraryService) { }

    public ngOnInit() {
        this.categoryService.getListCategory().subscribe();
        this.supplierService.getListSupplier().subscribe();
        this.publisherService.getListPublisher().subscribe();
        this.libraryService.getListLibrary().subscribe();
    }

    private selectTab(tab: number): void {
        this.selectedTab = tab;
    }
}
