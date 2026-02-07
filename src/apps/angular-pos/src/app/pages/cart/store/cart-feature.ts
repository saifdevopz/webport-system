import { createFeature, createReducer, createSelector, on } from '@ngrx/store';
import { cartActions } from './cart-actions';
import { CartItem } from '../cart-type';

export type CartState = {
  items: CartItem[];
  loading: boolean;
  error: string | null;
};

export const initialCartState: CartState = {
  items: [],
  loading: false,
  error: null,
};

export const cartFeature = createFeature({
  name: 'cart',
  reducer: createReducer(
    initialCartState,

    // Load cart
    on(cartActions.load, (state) => ({
      ...state,
      loading: true,
    })),

    on(cartActions.loadSuccess, (state, { items }) => ({
      ...state,
      items,
      loading: false,
      error: null,
    })),

    on(cartActions.loadFailure, (state, { error }) => ({
      ...state,
      loading: false,
      error,
    })),

    // Add to cart
    on(cartActions.addToCartSuccess, (state, { product }) => {
      const existingItem = state.items.find((item) => item.product.id === product.id);
      if (existingItem) {
        return {
          ...state,
          items: state.items.map((item) =>
            item.product.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
          ),
        };
      }
      return {
        ...state,
        items: [...state.items, { product, quantity: 1 }],
      };
    }),

    on(cartActions.removeFromCart, (state, { productId }) => ({
      ...state,
      items: state.items.filter((item) => item.product.id !== productId),
    })),

    on(cartActions.updateQuantity, (state, { productId, quantity }) => ({
      ...state,
      items:
        quantity > 0
          ? state.items.map((item) =>
              item.product.id === productId ? { ...item, quantity } : item
            )
          : state.items.filter((item) => item.product.id !== productId),
    })),

    on(cartActions.clearCart, (state) => ({
      ...state,
      items: [],
    }))
  ),

  extraSelectors: ({ selectItems }) => ({
    selectCartCount: createSelector(selectItems, (items) =>
      items.reduce((total, item) => total + item.quantity, 0)
    ),

    selectCartTotal: createSelector(selectItems, (items) =>
      items.reduce((total, item) => total + item.product.price * item.quantity, 0)
    ),
  }),
});
