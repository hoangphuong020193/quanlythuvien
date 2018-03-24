import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import { Library } from '../models/library.model';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as libraryAction from '../store/actions/library.action';

@Injectable()
export class LibraryService {
    private apiURL: string = Config.getPath(PathController.Library);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getListLibrary(): Observable<Library[]> {
        return this.http.get(this.apiURL + '/ReturnListLibrary').pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new libraryAction.FetchLibrary(res));
                    return res as Library[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }

    public saveLibrary(library: Library): Observable<number> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/SaveLibrary/',
            library, { headers }).pipe(
                tap(
                    (res: any) => {
                        library.id = res;
                        this.store.dispatch(new libraryAction.SaveLibrary(library));
                        return res;
                    }
                ),
                catchError((err) => {
                    return Observable.of(null);
                }));
    }
}
