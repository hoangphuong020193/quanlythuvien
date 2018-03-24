import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import { BookInCartDetail, Book, BookInCart } from '../models/book.model';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as bookAction from '../store/actions/book.action';
import { LsHelper } from '../shareds/helpers/ls.helper';
import * as moment from 'moment';
import { BookStatus } from '../shareds/enums/book-status.enum';
import { Format } from '../shareds/constant/format.constant';

@Injectable()
export class CartService {
    private apiURL: string = Config.getPath(PathController.Cart);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getBookInCartForBorrow(): Observable<BookInCartDetail[]> {
        return this.http.get(this.apiURL + '/ReturnBookInCartForBorrow').pipe(
            tap(
                (res: BookInCartDetail[]) => {
                    return res as BookInCartDetail[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public getBookInCart(): Observable<BookInCart[]> {
        return this.store.select(fromRoot.getCurrentUser).first().mergeMap((user) => {
            if (user) {
                return this.http.get(this.apiURL + '/ReturnBookInCart').pipe(
                    tap(
                        (res: BookInCart[]) => {
                            this.store.dispatch(new bookAction.FetchBookInCart(res));
                            return res as BookInCart[];
                        }
                    ),
                    catchError((err) => {
                        return Observable.of(null);
                    }));
            } else {
                const result = LsHelper.getItem(LsHelper.BookInCartStorage) as BookInCart[];
                return Observable.of(result ? result : []);
            }

        });
    }

    public getBookInCartDetail(): Observable<BookInCartDetail[]> {
        return this.http.get(this.apiURL + '/ReturnBookInCartDetail').pipe(
            tap(
                (res: BookInCartDetail[]) => {
                    return res as BookInCartDetail[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public addBookToCart(bookIds: number[]): Observable<BookInCart[]> {
        return this.store.select(fromRoot.getCurrentUser).first().mergeMap((user) => {
            if (user) {
                let headers = new HttpHeaders();
                headers = headers.append('Content-Type', 'application/json; charset=utf-8');
                return this.http.post(this.apiURL + '/AddBookInCart', bookIds, { headers }).pipe(
                    tap(
                        (res: any) => {
                            const result: BookInCart[] = [];
                            if (res) {
                                res.forEach((x: BookInCart) => {
                                    const bookInCart: BookInCart = new BookInCart();
                                    bookInCart.id = x.id;
                                    bookInCart.bookId = x.bookId;
                                    bookInCart.status = x.status;
                                    bookInCart.modifiedDate = x.modifiedDate;
                                    this.store.dispatch(new bookAction.AddBookInCart(bookInCart));
                                    result.push(bookInCart);
                                });
                            }
                            return result;
                        }
                    ),
                    catchError((err) => {
                        console.error(err);
                        return Observable.of(false);
                    }));
            } else {
                const result: BookInCart[] = [];
                const listOrigin = LsHelper.getItem(LsHelper.BookInCartStorage) as BookInCart[];
                bookIds.forEach((x) => {
                    if (!listOrigin || !listOrigin.find((item) => item.bookId === x)) {
                        const bookInCart: BookInCart = new BookInCart();
                        bookInCart.id = 0;
                        bookInCart.bookId = x;
                        bookInCart.status = BookStatus.InOrder;
                        bookInCart.modifiedDate = moment().format(Format.DateTimeFormatJson);
                        this.store.dispatch(new bookAction.AddBookInCart(bookInCart));
                        result.push(bookInCart);
                    }
                });
                this.store.select(fromRoot.getBookInCart).first().subscribe((res) => {
                    LsHelper.save(LsHelper.BookInCartStorage, res);
                });
                return Observable.of(result);
            }
        });
    }

    public deleteBookToCart(bookId: number): Observable<Response> {
        return this.store.select(fromRoot.getCurrentUser).first().mergeMap((user) => {
            if (user) {
                return this.http.delete(this.apiURL + '/DeleteBookInCart/' + bookId).pipe(
                    tap(
                        (res: any) => {
                            this.store.dispatch(new bookAction.DeleteBookInCart(bookId));
                            return res;
                        }
                    ),
                    catchError((err) => {
                        console.error(err);
                        return Observable.of(false);
                    }));
            } else {
                const listOrigin = LsHelper.getItem(LsHelper.BookInCartStorage) as BookInCart[];
                this.store.dispatch(new bookAction.DeleteBookInCart(bookId));
                LsHelper.save(LsHelper.BookInCartStorage,
                    listOrigin.filter((x) => x.bookId !== bookId));
                return Observable.of(true);
            }
        });
    }

    public updateStatusBookInCart(bookId: number, status: number): Observable<BookInCartDetail> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(this.apiURL + '/UpdateStatusBookInCart/' + bookId
            , status, { headers }).pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new bookAction.UpdateStatusBookInCart({ bookId, status }));
                    return res;
                }
            ),
            catchError((err) => {
                console.error(err);
                return Observable.of(false);
            }));
    }

    public getSlotAvailable(): Observable<number> {
        return this.http.get(this.apiURL + '/ReturnSlotAvailable').pipe(
            tap(
                (res: number) => {
                    return res;
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public borrowBook(): Observable<any> {
        return this.http.get(this.apiURL + '/BorrowBook').pipe(
            tap(
                (res: any) => {
                    return res;
                }
            ),
            catchError((err) => {
                console.error(err);
                return Observable.of(false);
            }));
    }
}
