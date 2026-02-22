import { createFeature, createReducer, on } from '@ngrx/store';
import { profileActions } from './profile-actions';
import { UserProfile } from '../profile-types';

export type ProfileState = {
  profile: UserProfile | null;
  loading: boolean;
  error: string | null;
};

const initialProfileState: ProfileState = {
  profile: null,
  loading: false,
  error: null,
};

export const profileFeature = createFeature({
  name: 'profile',
  reducer: createReducer(
    initialProfileState,
    on(profileActions.load, (state) => ({
      ...state,
      loading: true,
      error: null,
    })),

    on(profileActions.loadSuccess, (state, { profile }) => ({
      ...state,
      loading: false,
      profile,
    })),

    on(profileActions.loadFailure, (state, { error }) => ({
      ...state,
      loading: false,
      error,
    }))
  ),
});
