import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { cartActions } from './cart-actions';
import { map, tap, withLatestFrom } from 'rxjs';
import { Store } from '@ngrx/store';
import { cartFeature } from './cart-feature';
import { Storage } from '../../../core/services/storage';

const CART_STORAGE_KEY = 'ngrxstore_cart';
export const loadCartEffect = createEffect(
  (actions$ = inject(Actions), storage = inject(Storage)) => {
    return actions$.pipe(
      ofType(cartActions.load),
      map(() => {
        const cartData = storage.getItem(CART_STORAGE_KEY);
        const items = cartData ? JSON.parse(cartData) : [];
        return cartActions.loadSuccess({ items });
      })
    );
  },
  {
    functional: true,
  }
);

export const addToCartEffect = createEffect(
  (actions$ = inject(Actions)) => {
    return actions$.pipe(
      ofType(cartActions.addToCart),
      map(({ product }) => cartActions.addToCartSuccess({ product }))
    );
  },
  {
    functional: true,
  }
);

// very import one

export const persistCartEffect = createEffect(
  (actions$ = inject(Actions), storage = inject(Storage), store = inject(Store)) => {
    return actions$.pipe(
      ofType(
        cartActions.addToCartSuccess,
        cartActions.removeFromCart,
        cartActions.updateQuantity,
        cartActions.clearCart
      ),
      withLatestFrom(store.select(cartFeature.selectItems)),
      tap(([, items]) => {
        storage.setItem(CART_STORAGE_KEY, JSON.stringify(items));
      })
    );
  },
  {
    functional: true,
    dispatch: false,
  }
);
