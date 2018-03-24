import { Category } from '../../../models';
import { State } from './category.reducer';

export const getCategory: (state: State) => Category[] = (state: State) => {
    return state ? state.categories : undefined;
};
