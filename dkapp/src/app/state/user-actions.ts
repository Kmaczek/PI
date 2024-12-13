import { createAction, props } from '@ngrx/store';
import { UserProfile } from './user-model';

export const loadUser = createAction('[User] Load User');
export const loadUserSuccess = createAction(
  '[User] Load User Success',
  props<{ user: UserProfile }>()
);
export const loadUserFailure = createAction('[User] Load User Failure', props<{ error: string }>());
export const clearUser = createAction('[User] Clear User');
