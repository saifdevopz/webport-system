import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { WooProduct } from '../product-model';

export const productActions = createActionGroup({
  source: 'Products',
  events: {
    load: emptyProps(),
    loadSuccess: props<{ products: WooProduct[] }>(),
    loadFailure: props<{ error: string }>(),

    search: props<{ searchQuery: string }>(),
  },
});
