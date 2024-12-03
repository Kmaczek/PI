// user.selectors.ts
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserState } from './user-reducer';
import { LoadingStatus } from './state-model';

export const selectUserState = createFeatureSelector<UserState>('user');

// Profile selectors
export const selectUserProfile = createSelector(
  selectUserState,
  (state) => state.profile
);

export const selectUserProfileData = createSelector(
  selectUserProfile,
  (profile) => profile.data
);

export const selectUserProfileStatus = createSelector(
  selectUserProfile,
  (profile) => profile.status
);

export const selectUserProfileError = createSelector(
  selectUserProfile,
  (profile) => profile.error
);

export const selectIsUserProfileLoading = createSelector(
  selectUserProfileStatus,
  (status) => status === LoadingStatus.LOADING
);

export const selectIsUserProfileLoaded = createSelector(
  selectUserProfileStatus,
  (status) => status === LoadingStatus.SUCCESS || status === LoadingStatus.FAILED
);

export const selectUserRoles = createSelector(
  selectUserProfile,
  (profile) => profile.data?.userRoles ?? []
);