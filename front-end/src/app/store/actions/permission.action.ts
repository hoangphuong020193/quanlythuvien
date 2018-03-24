import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';

export const ActionTypes = {
    PERMISSION: '[Permission] Fetch permission'
};

export class FetchPermission implements Action {
    public readonly type: string = ActionTypes.PERMISSION;
    constructor(public payload: any) { }
}

export type Actions = FetchPermission;
