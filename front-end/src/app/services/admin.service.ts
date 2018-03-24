import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import { PagedList, CategoryReport } from '../models/index';
import { UserNotReturnBook, ReadStatistic, BorrowStatus } from '../models/admin.model';

@Injectable()
export class AdminService {
    private apiURL: string = Config.getPath(PathController.Admin);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getListUserNotReturnBook(page: number, pageSize: number)
        : Observable<PagedList<UserNotReturnBook>> {
        return this.http.get(this.apiURL + '/ReturnListUserNotReturnBook/'
            + page + '/' + pageSize)
            .pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getListReadStatistic(
        page: number, pageSize: number, startDate: string,
        endDate: string, groupBy: number)
        : Observable<PagedList<ReadStatistic>> {
        return this.http.get(this.apiURL + '/ReturnListReadStatistic/'
            + page + '/' + pageSize + '?startDate=' + startDate
            + '&endDate=' + endDate + '&groupBy=' + groupBy)
            .pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getListBorrowStatus(
        page: number, pageSize: number, startDate: string,
        endDate: string, libraryId: number, status: number, searchString: string)
        : Observable<PagedList<BorrowStatus>> {
        return this.http.get(this.apiURL + '/ReturnBorrowStatus/'
            + page + '/' + pageSize + '?startDate=' + startDate
            + '&endDate=' + endDate + '&libraryId=' + libraryId
            + '&status=' + status + '&searchString=' + searchString)
            .pipe(
                tap(
                    (res: any) => {
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }

    public getCategoryReport(
        page: number, pageSize: number, searchString: string)
        : Observable<PagedList<CategoryReport>> {
        return this.http.get(this.apiURL + '/ReturnCategoryReport/'
            + page + '/' + pageSize + '?searchString=' + searchString)
            .pipe(
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
