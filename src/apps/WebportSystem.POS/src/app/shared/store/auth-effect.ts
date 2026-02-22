import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';
import { authActions } from './auth-actions';
import { catchError, map, of, switchMap } from 'rxjs';
import { Storage } from '../../core/services/storage';
import { Auth } from '../../core/services/auth';
import { extractToken } from '../utils/extractToken';
import { NgToastService } from 'ng-angular-popup';

export const loginEffect = createEffect(
  (
    actions$ = inject(Actions),
    authApi = inject(Auth),
    toast = inject(NgToastService),
    router = inject(Router),
    storage = inject(Storage),
  ) => {
    return actions$.pipe(
      ofType(authActions.login),
      switchMap((loginRequest) => {
        return authApi.login(loginRequest).pipe(
          map((response) => {
            console.log('Login response:', response);
            router.navigateByUrl('/products');
            toast.success('Login Successful', 'SUCCESS');
            storage.setItem('ngrxstore_token', response.token);
            const payload = extractToken(response.token);

            if (payload) {
              return authActions.loginSuccess({ token: response.token, userId: payload.sub });
            }
            return authActions.loginSuccess({ token: response.token, userId: null });
          }),
          catchError((error) => {
            toast.danger('Login Failed', 'ERROR');
            return of(authActions.loginFailure({ error: error.message }));
          }),
        );
      }),
    );
  },
  {
    functional: true,
  },
);

export const registerEffect = createEffect(
  (
    actions$ = inject(Actions),
    authApi = inject(Auth),
    toast = inject(NgToastService),
    router = inject(Router),
  ) => {
    return actions$.pipe(
      ofType(authActions.register),
      switchMap((registerRequest) => {
        return authApi.register(registerRequest).pipe(
          map(() => {
            router.navigateByUrl('/login');
            toast.success('Registration Successful', 'SUCCESS');
            return authActions.registerSuccess();
          }),
          catchError((error) => {
            toast.danger('Registration Failed', 'ERROR');
            return of(authActions.registerFailure({ error: error.message }));
          }),
        );
      }),
    );
  },
  {
    functional: true,
  },
);

export const logoutEffect = createEffect(
  (
    actions$ = inject(Actions),
    storage = inject(Storage),
    router = inject(Router),
    toast = inject(NgToastService),
  ) => {
    return actions$.pipe(
      ofType(authActions.logout),
      map(() => {
        storage.removeItem('ngrxstore_token');
        router.navigateByUrl('/login');
        toast.success('Logout Successful', 'SUCCESS');
        return authActions.logoutSuccess();
      }),
    );
  },
  {
    functional: true,
  },
);
