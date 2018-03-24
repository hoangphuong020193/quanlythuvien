import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { RouterService } from '../../services/router.service';
import * as userAction from '../../store/actions/user.action';
import * as fromRoot from '../../store/reducers';
import { HttpStatusCode } from '../enums';
import { JQueryHelper } from './jquery.helper';
import { JsHelper } from './js.helper';

@Injectable()
export class ResponseHandler {
    constructor(
        private store: Store<fromRoot.State>,
        private routerService: RouterService) { }

    public handleError(err: any): void {
        switch (err.status) {
            case HttpStatusCode.Unauthorized:
                this.store.dispatch(new userAction.Logout(null));
                break;
            case HttpStatusCode.Forbidden:
                this.routerService.permissionDenied();
                break;
            default:
                break;
        }

        JQueryHelper.hideLoading();
        JQueryHelper.hideLocalLoading();
    }

    public handleResponse(res: any): void { }
}
