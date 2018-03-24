import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { HttpStatusCode } from '../shareds/enums';
import { LsHelper } from '../shareds/helpers';
import { Config, PathController } from '../config';
import { Login, Token, User } from '../models';
import * as userAction from '../store/actions/user.action';
import * as fromRoot from '../store/reducers';
import { map, tap } from 'rxjs/operators';
import { catchError } from 'rxjs/operators/catchError';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class LoginService {
    private apiURL: string = Config.getPath(PathController.Account);

    constructor(
        private http: HttpClient,
        private jwtHelperService: JwtHelperService,
        private store: Store<fromRoot.State>) { }

    public checkUserToken(accessToken: string = LsHelper.getAccessToken()): Observable<boolean> {
        if (!accessToken) {
            return Observable.of(false);
        }
        const headers: HttpHeaders = new HttpHeaders();
        headers.set('Content-Type', 'application/json');
        headers.set('Authorization', 'Bearer ' + accessToken);
        return this.http.get(this.apiURL, { headers })
            .map((res: any) => {
                return res.json().userName !== '';
            })
            .catch((err) => {
                console.error(err);
                return Observable.of(false);
            });
    }

    public login(loginModel: Login): Observable<any> {
        let headers = new HttpHeaders();
        headers = headers.append('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(this.apiURL + '/Login', loginModel, { headers }).pipe(
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

    public isTokenExpired(): boolean {
        const accessToken: string = LsHelper.getAccessToken();
        return this.jwtHelperService.isTokenExpired(accessToken);
    }

    public isUserValid(storageUser: User = LsHelper.getUser() as User): boolean {
        if (!storageUser || !storageUser.accessToken
            || !storageUser.userName || !storageUser.userId) {
            return false;
        }
        return true;
    }

    public createUserFromToken(token: string): User {
        const user: User = this.parseUserFromToken(token);
        this.store.dispatch(new userAction.CreateUser(user));
        return user;
    }

    private parseUserFromToken(token: string): User {
        const user: User = new User();
        user.accessToken = token;
        try {
            const decodedToken: any = this.jwtHelperService.decodeToken(token);
            // tslint:disable-next-line:radix
            user.userId = parseInt(decodedToken.userId);
            user.userName = decodedToken.sub;
            user.displayName = decodedToken.dispName;
            user.firstName = decodedToken.fistName;
            user.middleName = decodedToken.middleName;
            user.lastName = decodedToken.lastName;
            return user;
        } catch (err) {
            console.error(err);
            return user;
        }
    }
}
