import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { Library } from '../../models/library.model';

export const ActionTypes = {
    FETCH_LIBRARY: '[Library] Fetch library',
    SAVE_LIBRARY: '[Library] Save library'
};

export class FetchLibrary implements Action {
    public readonly type: string = ActionTypes.FETCH_LIBRARY;
    constructor(public payload: any) { }
}

// tslint:disable-next-line:max-classes-per-file
export class SaveLibrary implements Action {
    public readonly type: string = ActionTypes.SAVE_LIBRARY;
    constructor(public payload: Library) { }
}

export type Actions = FetchLibrary;
