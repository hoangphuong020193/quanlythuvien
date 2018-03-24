
import { State } from './library.reducer';
import { Library } from '../../../models/library.model';

export const getLibrary: (state: State) => Library[] = (state: State) => {
    return state ? state.libraries : undefined;
};
