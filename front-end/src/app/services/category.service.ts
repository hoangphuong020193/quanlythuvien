import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import { Category } from '../models/category.model';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as categoryAction from '../store/actions/category.action';

@Injectable()
export class CategoryService {
    private apiURL: string = Config.getPath(PathController.Category);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getListCategory(): Observable<Category[]> {
        return this.http.get(this.apiURL + '/ReturnCategory').pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new categoryAction.FetchCategory(res));
                    return res as Category[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public saveCategory(category: Category): Observable<number> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/SaveCategory/',
            category, { headers }).pipe(
                tap(
                    (res: any) => {
                        category.id = res;
                        this.store.dispatch(new categoryAction.SaveCategory(category));
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }
}
