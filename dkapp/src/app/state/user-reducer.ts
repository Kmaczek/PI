import { createReducer, on } from '@ngrx/store';
import {
  createInitialState,
  LoadingState,
  setError,
  setLoading,
  setSuccess
} from './state-model';
import * as UserActions from './user-actions';
import { UserProfile } from './user-model';

export interface UserState {
  profile: LoadingState<UserProfile>;
}

export const initialUserState: UserState = {
  profile: createInitialState(),
};

export const userReducer = createReducer(
  initialUserState,
  on(
    UserActions.loadUser,
    (state): UserState => ({
      ...state,
      profile: setLoading(state.profile),
    })
  ),
  on(
    UserActions.loadUserSuccess,
    (state, { user }): UserState => ({
      ...state,
      profile: setSuccess(state.profile, user),
    })
  ),
  on(
    UserActions.loadUserFailure,
    (state, { error }): UserState => ({
      ...state,
      profile: setError(state.profile, error)
    })
  )
);
