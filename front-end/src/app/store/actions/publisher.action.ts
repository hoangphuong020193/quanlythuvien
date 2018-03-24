import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { Publisher } from '../../models';

export const ActionTypes = {
    FETCH_PUBLISHER: '[Publisher] Fetch publisher',
    SAVE_PUBLISHER: '[Publisher] Save publisher'
};

export class FetchPublisher implements Action {
    public readonly type: string = ActionTypes.FETCH_PUBLISHER;
    constructor(public payload: any) { }
}

// tslint:disable-next-line:max-classes-per-file
export class SavePublisher implements Action {
    public readonly type: string = ActionTypes.SAVE_PUBLISHER;
    constructor(public payload: Publisher) { }
}

export type Actions = FetchPublisher | SavePublisher;
