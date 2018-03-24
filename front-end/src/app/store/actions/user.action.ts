import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { User } from '../../models';

export const ActionTypes = {
    CREATE: '[User] Create User',
    LOGOUT: '[User] LogOut User',
    UPDATE: '[User] Update User'
};

export class CreateUser implements Action {
    public readonly type: string = ActionTypes.CREATE;
    constructor(public payload: User) { }
}

// tslint:disable-next-line:max-classes-per-file
export class UpdateUser implements Action {
    public readonly type: string = ActionTypes.UPDATE;
    constructor(public payload: User) { }
}

// tslint:disable-next-line:max-classes-per-file
export class Logout implements Action {
    public readonly type: string = ActionTypes.LOGOUT;
    constructor(public payload: any) { }
}

export type Actions = CreateUser
    | UpdateUser
    | Logout;
