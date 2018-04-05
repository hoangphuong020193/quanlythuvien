import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import { Observable } from 'rxjs';
import { RouterService } from '../services/router.service';
import { PermissionId } from '../shareds/enums/permission.enum';

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(
        private store: Store<fromRoot.State>,
        private routerService: RouterService) { }

    public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : Observable<boolean> | boolean {
        return this.store.select(fromRoot.getCurrentUser).mergeMap((user) => {
            if (user) {
                return this.store.select(fromRoot.getPermission).mergeMap((permissions) => {
                    if (permissions.some((x) => x.groupPermissionId === PermissionId.ADMIN)) {
                        return Observable.of(true);
                    }
                });
            }
        });
    }
}
