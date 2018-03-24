import { Injectable } from '@angular/core';
import { Action } from '@ngrx/store';
import { Book, BookInCart } from '../../models';

export const ActionTypes = {
    SELECTED_BOOK_CODE: '[Book] Selected book code',
    FETCH_BOOK_SELECTED: '[Book] Fetch book selected',
    FETCH_BOOK_IN_CART: '[Book] Fetch book in cart',
    ADD_BOOK_IN_CART: '[Book] Add book in cart',
    UPDATE_STATUS_BOOK_IN_CART: '[Book] Update status book in cart',
    DELETE_BOOK_IN_CART: '[Book] Delete book in cart',
    CLEAR_BOOK_IN_CART: '[Book] Clear book in cart'
};

export class SelectedBook implements Action {
    public readonly type: string = ActionTypes.SELECTED_BOOK_CODE;
    constructor(public payload: string) { }
}

// tslint:disable-next-line:max-classes-per-file
export class FetchBookSelected implements Action {
    public readonly type: string = ActionTypes.FETCH_BOOK_SELECTED;
    constructor(public payload: Book) { }
}

// tslint:disable-next-line:max-classes-per-file
export class FetchBookInCart implements Action {
    public readonly type: string = ActionTypes.FETCH_BOOK_IN_CART;
    constructor(public payload: BookInCart[]) { }
}

// tslint:disable-next-line:max-classes-per-file
export class AddBookInCart implements Action {
    public readonly type: string = ActionTypes.ADD_BOOK_IN_CART;
    constructor(public payload: BookInCart) { }
}

// tslint:disable-next-line:max-classes-per-file
export class UpdateStatusBookInCart implements Action {
    public readonly type: string = ActionTypes.UPDATE_STATUS_BOOK_IN_CART;
    constructor(public payload: { bookId: number, status: number }) { }
}

// tslint:disable-next-line:max-classes-per-file
export class DeleteBookInCart implements Action {
    public readonly type: string = ActionTypes.DELETE_BOOK_IN_CART;
    constructor(public payload: number) { }
}

// tslint:disable-next-line:max-classes-per-file
export class ClearBookInCart implements Action {
    public readonly type: string = ActionTypes.CLEAR_BOOK_IN_CART;
    constructor(public payload: any) { }
}

export type Actions = SelectedBook
    | FetchBookSelected
    | FetchBookInCart
    | AddBookInCart
    | UpdateStatusBookInCart
    | DeleteBookInCart
    | ClearBookInCart;
