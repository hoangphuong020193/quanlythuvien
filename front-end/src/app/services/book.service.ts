import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { Book, SearchBookResult, BookBorrowAmount } from '../models/book.model';
import { map, tap, catchError } from 'rxjs/operators';
import { MyBook } from '../models/my-book.model';
import { UserBookRequest } from '../models/user-book-request.model';
import { HttpStatusCode } from '../shareds/enums/statuscode.enum';
import { PagedList } from '../models/paged-list.model';

@Injectable()
export class BookService {
    private apiURL: string = Config.getPath(PathController.Book);

    constructor(
        private http: HttpClient) {
    }

    public getListNewBook(): Observable<Book[]> {
        return this.http.get(this.apiURL + '/ReturnListNewBook').pipe(
            tap(
                (res: any) => {
                    return res as Book[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public getBookDetailByCode(bookCode: string): Observable<Book> {
        return this.http.get(this.apiURL + '/ReturnBookDetail/' + bookCode).pipe(
            tap(
                (res: any) => {
                    return res as Book[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public userFavoriteBook(bookId: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/UserFavoriteBook/', bookId, { headers }).pipe(
            tap(
                (res: any) => {
                    return res;
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public getTopBookInSection(sectionType: string): Observable<Book[]> {
        return this.http.get(this.apiURL + '/ReturnTopBookInSection/10?sectionType='
            + sectionType).pipe(
                tap(
                    (res: any) => {
                        return res as Book[];
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public bookView(view: string, page: number, pageSize: number = 30)
        : Observable<SearchBookResult> {
        return this.http.get(this.apiURL + '/BookView/'
            + page + '/' + pageSize + '?view=' + view).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getListBookByCode(code: string, libraryId: number): Observable<MyBook[]> {
        return this.http.get(this.apiURL + '/ReturnListBookByCode/'
            + libraryId + '?code=' + code).pipe(
                tap(
                    (res: any) => {
                        return res as MyBook[];
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getRequestInfo(code: string): Observable<UserBookRequest> {
        return this.http.get(this.apiURL + '/ReturnRequestInfo?code=' + code).pipe(
            tap(
                (res: any) => {
                    return res as UserBookRequest;
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public takenBook(userId: number, bookCode: string, requestId: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(this.apiURL + '/TakenBook/',
            JSON.stringify({ userId, bookCode, requestId }), { headers }).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public returnBook(userId: number, bookCode: string, requestId: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(this.apiURL + '/ReturnBook/',
            JSON.stringify({ userId, bookCode, requestId }), { headers }).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public cancelBook(userId: number, bookCode: string, requestId: number): Observable<boolean> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(this.apiURL + '/CancelBook/',
            JSON.stringify({ userId, bookCode, requestId }), { headers }).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getListBook(search: string, page: number, pageSize: number = 10)
        : Observable<PagedList<Book>> {
        return this.http.get(this.apiURL + '/ReturnListBook/'
            + page + '/' + pageSize + '?search=' + search).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public checkBookCodeExists(bookId: number, bookCode: string)
        : Observable<boolean> {
        return this.http.get(this.apiURL + '/CheckBookCodeExists/' + bookId + '/' + bookCode).pipe(
            tap(
                (res: any) => {
                    return res;
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public saveBook(book: Book): Observable<Book> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/SaveBook/',
            book, { headers }).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public saveImage(fileToUpload: any, bookId: number) {
        const input: any = new FormData();
        input.append('file', fileToUpload);
        return this.http.post(this.apiURL + '/SaveImage/' + bookId, input);
    }

    public topBook(page: number, pageSize: number, startDate: string, endDate: string)
        : Observable<PagedList<BookBorrowAmount>> {
        return this.http.get(this.apiURL + '/TopBook/'
            + page.toString() + '/'
            + pageSize.toString() + '/'
            + startDate + '/'
            + endDate).pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }
}
