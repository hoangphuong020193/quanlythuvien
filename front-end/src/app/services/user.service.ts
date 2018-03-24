import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { Config, PathController } from '../config';
import { Permission } from '../models/index';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as permissionAction from '../store/actions/permission.action';
import { tap, catchError } from 'rxjs/operators';

@Injectable()
export class UserService {
    private apiURL: string = Config.getPath(PathController.User);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getUserPermission(): Observable<Permission[]> {
        return this.http.get(this.apiURL + '/ReturnUserPermission').pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new permissionAction.FetchPermission(res));
                    return res as Permission[];
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }
}
