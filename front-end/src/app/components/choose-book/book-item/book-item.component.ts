import { Component, OnInit, Input } from '@angular/core';
import { Book, User } from '../../../models/index';
import { RouterService } from '../../../services/router.service';
import { State } from '../../../store/reducers/book/index';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import * as bookAction from '../../../store/actions/book.action';
import { Config } from '../../../config';
import { CartService } from '../../../services/cart.service';
import { BookService } from '../../../services/book.service';
import { DialogService } from 'angularx-bootstrap-modal';
import { LoginPopupComponent } from '../../login/login.component';

@Component({
    selector: 'book-item',
    templateUrl: './book-item.component.html'
})
export class BookItemComponent implements OnInit {
    @Input('book') public book: Book;

    constructor(
        private routerService: RouterService,
        private store: Store<fromRoot.State>,
        private cartService: CartService,
        private bookService: BookService,
        private dialogService: DialogService) { }

    public ngOnInit(): void { }

    private navigateToBookDetail(): void {
        this.routerService.bookDetail(this.book.bookCode);
        this.store.dispatch(new bookAction.SelectedBook(this.book.bookCode));
    }

    private favoriteBook(): void {
        this.bookService.userFavoriteBook(this.book.bookId).subscribe();
    }
}
