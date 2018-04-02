import { Component, OnInit, Input } from '@angular/core';
import { Book, SearchBookResult } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';
import { ActivatedRoute } from '@angular/router';
import { RouterService } from '../../services/router.service';
import { Config } from '../../config';
import { CartService } from '../../services/cart.service';
import { DialogService } from 'angularx-bootstrap-modal';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';
import * as bookAction from '../../store/actions/book.action';
import { LoginPopupComponent } from '../login/login.component';
import { User } from '../../models/user.model';
import { JsHelper } from '../../shareds/helpers/js.helper';

@Component({
    selector: 'book-view',
    templateUrl: './book-view.component.html'
})

export class BookViewComponent implements OnInit {

    private listBook: Book[] = [];
    private listBookShow: SearchBookResult[] = [];
    private totalResult: number = 0;

    private pageCurrent: number = 1;
    private view: string = '';

    constructor(
        private bookService: BookService,
        private routerService: RouterService,
        private activatedRoute: ActivatedRoute,
        private cartService: CartService,
        private dialogService: DialogService,
        private store: Store<fromRoot.State>) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.activatedRoute.queryParams.subscribe((params) => {
            this.view = params['view'];
            this.loadBook(1);
        });
    }

    private loadBook(page: number): void {
        if (this.view === ''
            || this.view === null
            || this.view === undefined) {
            this.routerService.home();
        }

        this.pageCurrent = page;
        this.bookService.bookView(this.view, this.pageCurrent)
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

    private navigateToBookDetail(bookCode: string): void {
        this.routerService.bookDetail(bookCode);
        this.store.dispatch(new bookAction.SelectedBook(bookCode));
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
        this.bookService.bookView(this.view, this.pageCurrent)
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
