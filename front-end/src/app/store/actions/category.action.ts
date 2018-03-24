import { Category } from './../../models/category.model';
import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { Book } from '../../models';

export const ActionTypes = {
    FETCH_CATEGORY: '[Category] Fetch category',
    SAVE_CATEGORY: '[Category] Save category'
};

export class FetchCategory implements Action {
    public readonly type: string = ActionTypes.FETCH_CATEGORY;
    constructor(public payload: any) { }
}

// tslint:disable-next-line:max-classes-per-file
export class SaveCategory implements Action {
    public readonly type: string = ActionTypes.SAVE_CATEGORY;
    constructor(public payload: Category) { }
}

export type Actions = FetchCategory;
