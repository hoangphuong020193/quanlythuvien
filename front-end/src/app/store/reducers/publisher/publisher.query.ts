import { Category } from '../../../models';
import { State } from './publisher.reducer';
import { Publisher } from '../../../models/publisher.model';

export const getPublisher: (state: State) => Publisher[] = (state: State) => {
    return state ? state.publishers : undefined;
};
