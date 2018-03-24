import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';

export const ActionTypes = {
    FETCH_NOTIFICATION: '[Notification] Fetch notification'
};

export class FetchNotification implements Action {
    public readonly type: string = ActionTypes.FETCH_NOTIFICATION;
    constructor(public payload: any) { }
}

export type Actions = FetchNotification;
