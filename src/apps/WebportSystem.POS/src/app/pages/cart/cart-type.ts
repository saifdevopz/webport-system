import { Product } from '../products/product-model';

export type CartItem = {
  product: Product;
  quantity: number;
};
