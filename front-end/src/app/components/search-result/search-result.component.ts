import { Component, OnInit } from '@angular/core';
import { RouterService } from '../../services/router.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';
import * as bookAction from '../../store/actions/book.action';
import { SearchBookService } from '../../services/search-book.service';
import { Book, SearchBookResult } from '../../models/book.model';
import { ActivatedRoute } from '@angular/router';
import { JsHelper } from '../../shareds/helpers/js.helper';
import { User } from '../../models/user.model';
import { DialogService } from 'angularx-bootstrap-modal';
import { LoginPopupComponent } from '../../components/login/login.component';
import { CartService } from '../../services/cart.service';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';
import { Config } from '../../config';

@Component({
    selector: 'search-result',
    templateUrl: './search-result.component.html'
})
export class SearchResultComponent implements OnInit {

    private listBook: Book[] = [];
    private listBookShow: SearchBookResult[] = [];
    private totalResult: number = 0;

    private pageCurrent: number = 1;
    private searchString: string = '';

    constructor(
        private routerService: RouterService,
        private activatedRoute: ActivatedRoute,
        private searchBookService: SearchBookService,
        private cartService: CartService,
        private dialogService: DialogService,
        private store: Store<fromRoot.State>) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.activatedRoute.queryParams.subscribe((params) => {
            this.searchString = params['search'];
            $('#search-box').val(this.searchString);
            this.searchBook(1);
        });
    }

    private navigateToBookDetail(bookCode: string): void {
        this.routerService.bookDetail(bookCode);
        this.store.dispatch(new bookAction.SelectedBook(bookCode));
    }

    private searchBook(page: number): void {

        if (this.searchString === ''
            || this.searchString === null
            || this.searchString === undefined) {
            this.routerService.home();
        }

        this.pageCurrent = page;
        this.searchBookService.searchBook(this.searchString, this.pageCurrent)
            .subscribe((res: SearchBookResult) => {
                if (res) {
                    this.listBook = res.listBooks;
                    this.totalResult = res.total;
                    let listTemp: Book[] = [];
                    this.listBookShow = [];
                    let index: number = 1;
                    this.listBook.forEach((book) => {
                        listTemp.push(book);
                        if (listTemp.length === 5 || index === this.listBook.length) {
                            this.listBookShow.push(
                                new SearchBookResult(JsHelper.cloneObject(listTemp)));
                            listTemp = [];
                        }
                        index++;
                    });
                    JQueryHelper.hideLoading();
                }
            });
    }

    private addBookToCart(bookId: number): void {
        this.store.select(fromRoot.getCurrentUser).subscribe((user: User) => {
            if (!user) {
                this.dialogService.addDialog(LoginPopupComponent, {
                    reloadPage: false
                }).subscribe((res) => {
                    if (res) {
                        this.cartService.addBookToCart([bookId]).subscribe();
                    }
                });
            } else {
                this.cartService.addBookToCart([bookId]).subscribe();
            }
        });
    }

    private loadMoreBook(): void {
        JQueryHelper.showLoading();
        this.pageCurrent++;
        this.searchBookService.searchBook(this.searchString, this.pageCurrent)
            .subscribe((res: SearchBookResult) => {
                if (res) {
                    this.listBook.push(...res.listBooks);
                    let listTemp: Book[] = [];
                    this.listBookShow = [];
                    let index: number = 1;
                    this.listBook.forEach((book) => {
                        listTemp.push(book);
                        if (listTemp.length === 5 || index === this.listBook.length) {
                            this.listBookShow.push(
                                new SearchBookResult(JsHelper.cloneObject(listTemp)));
                            listTemp = [];
                        }
                        index++;
                    });
                    JQueryHelper.hideLoading();
                }
            });
    }
}
