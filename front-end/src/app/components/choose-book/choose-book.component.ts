import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';
import { Observable } from 'rxjs';
import { Category } from '../../models/index';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';
import { BookSection } from '../../shareds/constant/book-section.constant';
import { RouterService } from '../../services/router.service';

@Component({
    selector: 'choose-book',
    templateUrl: './choose-book.component.html'
})
export class ChooseBookComponent implements OnInit {

    private listCategories: Category[] = [];
    private BookSection: any = BookSection;
    private randomSection: Category[] = [];

    constructor(
        private categoryService: CategoryService,
        private routerService: RouterService,
        private store: Store<fromRoot.State>) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.categoryService.getListCategory().subscribe();
        this.store.select(fromRoot.getCategory).subscribe((res) => {
            this.listCategories = res;
            JQueryHelper.hideLoading();

            while (this.randomSection.length < this.listCategories.length
                && this.randomSection.length <= 5) {
                const index: number = Math.floor(Math.random()
                    * (this.listCategories.length - 0) + 0);
                const category: Category = this.listCategories[index];
                if (!this.randomSection.some((x) => x.id === category.id)) {
                    this.randomSection.push(category);
                }
            }
        });
    }

    private showType(index: number): boolean {
        if (index === 0
            || this.listCategories[index - 1].type !== this.listCategories[index].type) {
            return true;
        }
        return false;
    }

    private navigateToBookView(categoryName: string): void {
        this.routerService.bookView(categoryName);
    }
}
