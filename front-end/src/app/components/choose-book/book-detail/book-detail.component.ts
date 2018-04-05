import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { BookService } from '../../../services/book.service';
import { Book, User } from '../../../models/index';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { RouterService } from '../../../services/router.service';
import { Config } from '../../../config';
import { CartService } from '../../../services/cart.service';
import { DialogService } from 'angularx-bootstrap-modal';
import { LoginPopupComponent } from '../../login/login.component';
import * as moment from 'moment';
import { Format } from '../../../shareds/constant/format.constant';

@Component({
    selector: 'book-detail',
    templateUrl: './book-detail.component.html'
})

export class BookDetailComponent implements OnInit {
    private book: Book = new Book();

    constructor(
        private store: Store<fromRoot.State>,
        private bookService: BookService,
        private cartService: CartService,
        private dialogService: DialogService,
        private routerService: RouterService) { }

    public ngOnInit() {
        JQueryHelper.showLoading();
        this.store.select(fromRoot.getSelectedBookCode).first().subscribe((bookCode) => {
            if (bookCode) {
                this.bookService.getBookDetailByCode(bookCode).subscribe((res) => {
                    if (res) {
                        this.book = res;
                        JQueryHelper.hideLoading();
                    } else {
                        this.routerService.home();
                    }
                });
            }
        });
    }

    private addBookToCart(): void {
        this.store.select(fromRoot.getCurrentUser).subscribe((user: User) => {
            if (!user) {
                this.dialogService.addDialog(LoginPopupComponent, {
                    reloadPage: false
                }).subscribe((res) => {
                    if (res) {
                        this.cartService.addBookToCart([this.book.bookId]).subscribe();
                        this.bookService.getBookDetailByCode(this.book.bookCode)
                            .subscribe((book) => {
                                this.book = book;
                            });
                    }
                });
            } else {
                this.cartService.addBookToCart([this.book.bookId]).subscribe();
            }
        });
    }

    private favoriteBook(): void {
        this.store.select(fromRoot.getCurrentUser).subscribe((user: User) => {
            if (!user) {
                this.dialogService.addDialog(LoginPopupComponent, {
                    reloadPage: false
                }).subscribe((res) => {
                    if (res) {
                        this.bookService.userFavoriteBook(this.book.bookId).subscribe();
                    }
                });
            } else {
                this.bookService.userFavoriteBook(this.book.bookId).subscribe();
            }
        });
    }

    private parseDateToString(date: Date): string {
        return moment(date).format(Format.DateFormat);
    }

    private generateQR(): string {
        return 'Book Code: ' + this.book.bookCode
            + '::Book Name: ' + this.book.bookName
            + '::Author:' + this.book.author;
    }
}
