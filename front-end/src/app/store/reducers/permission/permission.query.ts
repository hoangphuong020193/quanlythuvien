import { State } from './permission.reducer';
import { Permission } from '../../../models/permission.model';

export const getPermission: (state: State) => Permission[] = (state: State) => {
    return state ? state.permission : undefined;
};
