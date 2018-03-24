import { Action, ActionReducer } from '@ngrx/store';
import { User } from '../../../models';
import * as userAction from '../../actions/user.action';

export interface State {
    user: User;
}

export const initialState: State = {
    user: null
};

export function reducer(state: State = initialState, action: userAction.Actions): State {
    switch (action.type) {
        case userAction.ActionTypes.CREATE:
            return Object.assign({}, state, { user: action.payload });

        case userAction.ActionTypes.LOGOUT:
            return Object.assign({}, state, {
                user: Object.assign({},
                    state.user,
                    {
                        isLoggedOut: true
                    })
            });

        case userAction.ActionTypes.UPDATE:
            return Object.assign({}, state, { user: action.payload });

        default:
            return state;
    }
}
