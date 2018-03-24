import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import * as fromRoot from '../store/reducers';
import { SearchBookResult } from '../models/book.model';

@Injectable()
export class SearchBookService {
    private apiURL: string = Config.getPath(PathController.SearchBook);

    constructor(
        private http: HttpClient) {
    }

    public searchBook(search: string, page: number, pageSize: number = 30)
        : Observable<SearchBookResult> {
        return this.http.get(this.apiURL + '/SearchBook/'
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
}
