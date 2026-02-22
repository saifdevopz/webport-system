import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { profileActions } from './profile-actions';
import { catchError, map, of, switchMap } from 'rxjs';
import { ProfileApi } from '../profile-api';

export const profileEffect = createEffect(
  (actions$ = inject(Actions), profileApi = inject(ProfileApi)) => {
    return actions$.pipe(
      ofType(profileActions.load),
      switchMap(({ userId }) => {
        return profileApi.getUserProfile(userId).pipe(
          map((profile) => profileActions.loadSuccess({ profile })),
          catchError((error) => of(profileActions.loadFailure({ error: error.message })))
        );
      })
    );
  },
  {
    functional: true,
  }
);
