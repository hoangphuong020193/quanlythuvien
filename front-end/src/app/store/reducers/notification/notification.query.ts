import { State } from './notification.reducer';
import { Notifications } from '../../../models/notification.model';

export const getNotification: (state: State) => Notifications[] = (state: State) => {
    return state ? state.notification : undefined;
};
