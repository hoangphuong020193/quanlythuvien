import { Action, ActionReducer } from '@ngrx/store';
import { Category } from '../../../models';
import * as categoryAction from '../../actions/category.action';

export interface State {
    categories: Category[];
}

export const initialState: State = {
    categories: []
};

export function reducer(state: State = initialState, action: categoryAction.Actions): State {
    switch (action.type) {
        case categoryAction.ActionTypes.FETCH_CATEGORY:
            return Object.assign({}, state, { categories: action.payload });
        case categoryAction.ActionTypes.SAVE_CATEGORY:
            const index: number = state.categories.findIndex((x) => x.id === action.payload.id);
            if (index > -1) {
                state.categories[index] = action.payload;
            } else {
                state.categories.push(action.payload);
            }
            return state;
        default:
            return state;
    }
}
