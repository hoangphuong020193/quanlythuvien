import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';
import { Notifications } from '../../models/index';
import * as moment from 'moment';
import { Format } from '../../shareds/constant/format.constant';

@Component({
    selector: 'notification',
    templateUrl: './notification.component.html'
})
export class NotificationComponent implements OnInit {
    private listNotifications: Notifications[] = [];

    constructor(
        private store: Store<fromRoot.State>) { }

    public ngOnInit(): void {
        this.store.select(fromRoot.getNotification).subscribe((res) => {
            this.listNotifications = res;
        });
    }

    private parseDateToString(date: Date): string {
        return moment(date).format(Format.DateTimeFormat);
    }
}
