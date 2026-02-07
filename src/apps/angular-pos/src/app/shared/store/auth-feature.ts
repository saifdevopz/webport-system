import { createFeature, createReducer, createSelector, on } from '@ngrx/store';
import { authActions } from './auth-actions';

export type AuthState = {
  token: string | null;
  userId: number | null;
  error: string | null;
  isLoading: boolean;
};

const getStoredToken = (): string | null => {
  return localStorage.getItem('ngrxstore_token');
};

export const initialAuthState: AuthState = {
  token: getStoredToken(),
  userId: null,
  error: null,
  isLoading: false,
};

export const authFeatures = createFeature({
  name: 'auth',
  reducer: createReducer(
    initialAuthState,

    on(authActions.loginSuccess, (state, { token, userId }) => ({
      ...state,
      token,
      userId,
      isLoading: false,
    })),

    on(authActions.loginFailure, (state, { error }) => ({
      ...state,
      token: null,
      error,
    })),

    on(authActions.login, (state) => ({
      ...state,
      isLoading: true,
      error: null,
    })),

    on(authActions.register, (state) => ({
      ...state,
      isLoading: true,
      error: null,
    })),

    on(authActions.registerSuccess, (state) => ({
      ...state,
      isLoading: false,
    })),

    on(authActions.registerFailure, (state, { error }) => ({
      ...state,
      isLoading: false,
      error,
    })),

    on(authActions.logoutSuccess, (state) => ({
      ...state,
      token: null,
      userId: null,
      isLoading: false,
    })),
  ),

  extraSelectors: ({ selectToken }) => ({
    selectIsAuthenticated: createSelector(selectToken, (token) => !!token),
  }),
});
