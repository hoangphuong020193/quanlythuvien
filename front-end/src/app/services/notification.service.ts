import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config, PathController } from '../config';
import { map, tap, catchError } from 'rxjs/operators';
import * as fromRoot from '../store/reducers';
import { Notifications } from '../models/notification.model';
import { Store } from '@ngrx/store';
import * as notificationAction from '../store/actions/notification.action';

@Injectable()
export class NotificationService {
    private apiURL: string = Config.getPath(PathController.User);

    constructor(
        private http: HttpClient,
        private store: Store<fromRoot.State>) {
    }

    public getNotification()
        : Observable<Notifications[]> {
        return this.http.get(this.apiURL + '/ReturnUserNotification/').pipe(
            tap(
                (res: any) => {
                    this.store.dispatch(new notificationAction.FetchNotification(res));
                    return res;
                }
            ),
            catchError((err) => {
                return Observable.of(null);
            }));
    }
}
