import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { Supplier } from '../../models';

export const ActionTypes = {
    FETCH_SUPPLIER: '[Supplier] Fetch supplier',
    SAVE_SUPPLIER: '[Supplier] Save supplier',
};

export class FetchSupplier implements Action {
    public readonly type: string = ActionTypes.FETCH_SUPPLIER;
    constructor(public payload: any) { }
}

// tslint:disable-next-line:max-classes-per-file
export class SaveSupplier implements Action {
    public readonly type: string = ActionTypes.SAVE_SUPPLIER;
    constructor(public payload: Supplier) { }
}

export type Actions = FetchSupplier | SaveSupplier;
