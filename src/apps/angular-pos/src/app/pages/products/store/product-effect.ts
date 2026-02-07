import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { productActions } from './product-actions';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { ProductApi } from '../product-api';

export const productEffect = createEffect(
  (action$ = inject(Actions), productApi = inject(ProductApi)) => {
    return action$.pipe(
      ofType(productActions.load),
      switchMap(() => {
        return productApi.getProducts().pipe(
          tap((products) => {
            console.log('Products JSON:', products);
          }),
          map((products) => productActions.loadSuccess({ products })),
          catchError((error) => of(productActions.loadFailure({ error: error.message })))
        );
      })
    );
  },
  {
    functional: true,
  }
);
