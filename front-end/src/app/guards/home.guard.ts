import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as userAction from '../store/actions/user.action';
import { LoginService } from '../services/login.service';
import { User } from '../models/user.model';
import { LsHelper } from '../shareds/helpers/ls.helper';
import { map } from 'rxjs/operators';
import { UserService } from '../services/user.service';

@Injectable()
export class HomeGuard implements CanActivate {
    constructor(
        private store: Store<fromRoot.State>,
        private loginService: LoginService,
        private userService: UserService) {
    }

    public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : Observable<boolean> | boolean {
        this.store.select(fromRoot.getCurrentUser).first().subscribe((user) => {
            const storageUser: User = LsHelper.getUser() as User;
            if (this.loginService.isUserValid(storageUser)
                && !this.loginService.isTokenExpired()) {
                this.store.dispatch(new userAction.CreateUser(storageUser));
                this.userService.getUserPermission().subscribe();
            } else {
                LsHelper.clearStorage();
            }
        });
        return Observable.of(true);
    }
}
