import { Category } from '../../../models';
import { State } from './supplier.reducer';
import { Supplier } from '../../../models/supplier.model';

export const getSupplier: (state: State) => Supplier[] = (state: State) => {
    return state ? state.suppliers : undefined;
};
