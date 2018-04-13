import { Component, OnInit } from '@angular/core';
import { Config } from '../../../config';
import { CartService } from '../../../services/cart.service';
import { BookInCartDetail } from '../../../models/index';
import { BookStatus } from '../../../shareds/enums/book-status.enum';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { RouterService } from '../../../services/router.service';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';

@Component({
    selector: 'book-in-cart',
    templateUrl: './book-in-cart.component.html'
})
export class BookInCartComponent implements OnInit {
    private bookAvailable: BookInCartDetail[] = [];
    private bookWaiting: BookInCartDetail[] = [];
    private BookStatus: any = BookStatus;

    constructor(
        private cartService: CartService,
        private store: Store<fromRoot.State>,
        private route: RouterService) { }

    public ngOnInit(): void {
        this.cartService.getBookInCart().subscribe();
        this.cartService.getBookInCartDetail().subscribe((res) => {
            if (res) {
                this.bookAvailable = res.filter((x) => x.status === BookStatus.InOrder);
                this.bookWaiting = res.filter((x) => x.status === BookStatus.Waiting);
                JQueryHelper.hideLoading();
            }
        });
    }

    private deleteBookInCart(bookId: number): void {
        this.cartService.deleteBookToCart(bookId).subscribe((res) => {
            this.bookAvailable = this.bookAvailable.filter((x) => x.bookId !== bookId);
            this.bookWaiting = this.bookWaiting.filter((x) => x.bookId !== bookId);
        });
    }

    private changeStatusBook(bookId: number, status: number): void {
        this.cartService.updateStatusBookInCart(bookId, status).subscribe((res) => {
            let book: BookInCartDetail = this.bookAvailable.find((x) => x.bookId === bookId);
            if (!book) {
                book = this.bookWaiting.find((x) => x.bookId === bookId);
                book.status = status;
                this.bookAvailable.push(book);
                this.bookWaiting = this.bookWaiting.filter((x) => x.bookId !== bookId);
            } else {
                book.status = status;
                this.bookWaiting.push(book);
                this.bookAvailable = this.bookAvailable.filter((x) => x.bookId !== bookId);
            }
        });
    }

    private navigateToHome(): void {
        this.route.home();
    }
}
