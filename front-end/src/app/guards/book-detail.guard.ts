import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../store/reducers';
import * as bookAction from '../store/actions/book.action';
import { Observable } from 'rxjs';
import { RouterService } from '../services/router.service';

@Injectable()
export class BookDetailGuard implements CanActivate {
    constructor(
        private store: Store<fromRoot.State>,
        private routerService: RouterService) { }

    public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
        : Observable<boolean> | boolean {
        return this.store.select(fromRoot.getSelectedBookCode).mergeMap((bookCode) => {
            const code: string = next.params['bookCode'] !== '' ?
                next.params['bookCode'] : bookCode !== '' ? bookCode : '';
            if (code === '') {
                this.routerService.home();
            }
            this.store.dispatch(new bookAction.SelectedBook(code));
            return Observable.of(true);
        });
    }
}
