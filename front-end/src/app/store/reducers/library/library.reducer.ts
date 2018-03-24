import { Action, ActionReducer } from '@ngrx/store';
import * as libraryAction from '../../actions/library.action';
import { Library } from '../../../models/library.model';

export interface State {
    libraries: Library[];
}

export const initialState: State = {
    libraries: []
};

export function reducer(state: State = initialState, action: libraryAction.Actions): State {
    switch (action.type) {
        case libraryAction.ActionTypes.FETCH_LIBRARY:
            return Object.assign({}, state, { libraries: action.payload });
        case libraryAction.ActionTypes.SAVE_LIBRARY:
            const index: number = state.libraries.findIndex((x) => x.id === action.payload.id);
            if (index > -1) {
                state.libraries[index] = action.payload;
            } else {
                state.libraries.push(action.payload);
            }
            return state;
        default:
            return state;
    }
}
