import { User } from '../../../models/user.model';
import { State } from './user.reducer';

export const getCurrentUser: (state: State) => User = (state: State) => {
    return state ? state.user : undefined;
};
