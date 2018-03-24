import { Action, ActionReducer } from '@ngrx/store';
import * as publisherAction from '../../actions/publisher.action';
import { Publisher } from '../../../models/index';

export interface State {
    publishers: Publisher[];
}

export const initialState: State = {
    publishers: []
};

export function reducer(state: State = initialState, action: publisherAction.Actions): State {
    switch (action.type) {
        case publisherAction.ActionTypes.FETCH_PUBLISHER:
            return Object.assign({}, state, { publishers: action.payload });
        case publisherAction.ActionTypes.SAVE_PUBLISHER:
            const index: number = state.publishers.findIndex((x) => x.id === action.payload.id);
            if (index > -1) {
                state.publishers[index] = action.payload;
            } else {
                state.publishers.push(action.payload);
            }
            return state;
        default:
            return state;
    }
}
