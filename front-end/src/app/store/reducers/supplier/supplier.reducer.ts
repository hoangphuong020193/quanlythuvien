import { Action, ActionReducer } from '@ngrx/store';
import * as supplierAction from '../../actions/supplier.action';
import { Supplier } from '../../../models/supplier.model';

export interface State {
    suppliers: Supplier[];
}

export const initialState: State = {
    suppliers: []
};

export function reducer(state: State = initialState, action: supplierAction.Actions): State {
    switch (action.type) {
        case supplierAction.ActionTypes.FETCH_SUPPLIER:
            return Object.assign({}, state, { suppliers: action.payload });
        case supplierAction.ActionTypes.SAVE_SUPPLIER:
            const index: number = state.suppliers.findIndex((x) => x.id === action.payload.id);
            if (index > -1) {
                state.suppliers[index] = action.payload;
            } else {
                state.suppliers.push(action.payload);
            }
        default:
            return state;
    }
}
