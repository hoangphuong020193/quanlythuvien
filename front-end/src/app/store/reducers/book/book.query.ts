import { Book, BookInCart } from '../../../models';
import { State } from './book.reducer';

export const getSelectedBookCode: (state: State) => string = (state: State) => {
    return state ? state.bookCodeSelected : undefined;
};

export const getBookSelected: (state: State) => Book = (state: State) => {
    return state ? state.bookSelected : undefined;
};

export const getBookInCart: (state: State) => BookInCart[] = (state: State) => {
    return state ? state.bookInCart : undefined;
};
