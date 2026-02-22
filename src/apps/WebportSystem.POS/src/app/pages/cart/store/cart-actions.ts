import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { CartItem } from '../cart-type';
import { WooProduct } from '../../products/product-model';

export const cartActions = createActionGroup({
  source: 'Cart',
  events: {
    load: emptyProps(),
    loadSuccess: props<{ items: CartItem[] }>(),
    loadFailure: props<{ error: string }>(),

    addToCart: props<{ product: WooProduct }>(),
    addToCartSuccess: props<{ product: WooProduct }>(),
    addToCartFailure: props<{ error: string }>(),

    removeFromCart: props<{ productId: number }>(),

    updateQuantity: props<{ productId: number; quantity: number }>(),

    clearCart: emptyProps(),
  },
});
