import { createFeature, createReducer, on } from '@ngrx/store';
import { productActions } from './product-actions';
import { Product } from '../product-type';

export type ProductState = {
  products: Product[]; // 20 records
  filteredProducts: Product[]; // filtered records based on search
  searchQuery: string | null;
  error: string | null;
  loading: boolean;
};

export const initialProductState: ProductState = {
  products: [],
  filteredProducts: [],
  searchQuery: null,
  error: null,
  loading: false,
};

export const productFeature = createFeature({
  name: 'products',
  reducer: createReducer(
    initialProductState,

    on(productActions.load, (state) => ({
      ...state,
      loading: true,
    })),

    on(productActions.loadSuccess, (state, { products }) => ({
      ...state,
      products,
      filteredProducts: products,
      loading: false,
      error: null,
    })),

    on(productActions.loadFailure, (state, { error }) => ({
      ...state,
      error,
      loading: false,
    })),

    on(productActions.search, (state, { searchQuery }) => {
      const filteredProducts = state.products.filter((product) =>
        product.title.toLowerCase().includes(searchQuery.toLowerCase())
      );
      return {
        ...state,
        searchQuery,
        filteredProducts,
      };
    })
  ),
});
