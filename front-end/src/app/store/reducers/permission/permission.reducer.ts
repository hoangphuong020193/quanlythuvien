import { Action, ActionReducer } from '@ngrx/store';
import * as permissionAction from '../../actions/permission.action';
import { Permission } from '../../../models/index';

export interface State {
    permission: Permission[];
}

export const initialState: State = {
    permission: []
};

export function reducer(state: State = initialState, action: permissionAction.Actions): State {
    switch (action.type) {
        case permissionAction.ActionTypes.PERMISSION:
            return Object.assign({}, state, { permission: action.payload });
        default:
            return state;
    }
}
