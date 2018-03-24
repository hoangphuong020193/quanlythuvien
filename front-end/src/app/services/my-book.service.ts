import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import * as fromRoot from '../store/reducers';
import { MyBook } from '../models/my-book.model';
import { PagedList } from '../models/paged-list.model';

@Injectable()
export class MyBookService {
    private apiURL: string = Config.getPath(PathController.MyBook);

    constructor(
        private http: HttpClient) {
    }

    public getListMyBook(status: string, page: number, pageSize: number = 10)
        : Observable<PagedList<MyBook>> {
        return this.http.get(this.apiURL + '/ReturnMyBookList/'
            + page + '/' + pageSize + '?status=' + status).pipe(
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
