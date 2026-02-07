import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { Product } from '../product-type';

export const productActions = createActionGroup({
  source: 'Products',
  events: {
    load: emptyProps(),
    loadSuccess: props<{ products: Product[] }>(),
    loadFailure: props<{ error: string }>(),

    search: props<{ searchQuery: string }>(),
  },
});
