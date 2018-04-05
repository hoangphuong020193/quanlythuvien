import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import { Observable } from 'rxjs';
import { RouterService } from '../services/router.service';

@Injectable()
export class BookInCartCheckGuard implements CanActivate {
    constructor(
        private store: Store<fromRoot.State>,
        private routerService: RouterService) { }

    public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : Observable<boolean> | boolean {
        return this.store.select(fromRoot.getCurrentUser).mergeMap((user) => {
            if (user) {
                return Observable.of(true);
            } else {
                this.routerService.home();
            }
        });
    }
}
