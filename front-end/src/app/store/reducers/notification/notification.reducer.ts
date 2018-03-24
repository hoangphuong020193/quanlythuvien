import { Action, ActionReducer } from '@ngrx/store';
import { Notifications } from '../../../models';
import * as notificationAction from '../../actions/notification.action';

export interface State {
    notification: Notifications[];
}

export const initialState: State = {
    notification: []
};

export function reducer(state: State = initialState, action: notificationAction.Actions): State {
    switch (action.type) {
        case notificationAction.ActionTypes.FETCH_NOTIFICATION:
            return Object.assign({}, state, { notification: action.payload });
        default:
            return state;
    }
}
