import { Action, ActionReducer } from '@ngrx/store';
import { Book, BookInCart } from '../../../models';
import * as bookAction from '../../actions/book.action';

export interface State {
    bookCodeSelected: string;
    bookSelected: Book;
    bookInCart: BookInCart[];
}

export const initialState: State = {
    bookCodeSelected: '',
    bookSelected: null,
    bookInCart: []
};

export function reducer(state: State = initialState, action: bookAction.Actions): State {
    switch (action.type) {
        case bookAction.ActionTypes.SELECTED_BOOK_CODE:
            return Object.assign({}, state, { bookCodeSelected: action.payload });
        case bookAction.ActionTypes.FETCH_BOOK_SELECTED:
            return Object.assign({}, state, { bookSelected: action.payload });
        case bookAction.ActionTypes.FETCH_BOOK_IN_CART:
            return Object.assign({}, state, { bookInCart: action.payload });
        case bookAction.ActionTypes.ADD_BOOK_IN_CART:
            return Object.assign({}, state, {
                bookInCart: state.bookInCart.concat(action.payload)
            });
        case bookAction.ActionTypes.DELETE_BOOK_IN_CART:
            return Object.assign({}, state, {
                bookInCart: state.bookInCart.filter((x) => x.bookId !== action.payload)
            });
        case bookAction.ActionTypes.CLEAR_BOOK_IN_CART:
            return Object.assign({}, state, {
                bookInCart: []
            });
        case bookAction.ActionTypes.UPDATE_STATUS_BOOK_IN_CART:
            return Object.assign({}, state, {
                bookInCart: state.bookInCart.map((x) => {
                    if (x.bookId === action.payload.bookId) {
                        x.status = action.payload.status;
                    }
                    return x;
                })
            });
        default:
            return state;
    }
}
