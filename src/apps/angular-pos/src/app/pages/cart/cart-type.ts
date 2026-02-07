import { Product } from '../products/product-type';

export type CartItem = {
  product: Product;
  quantity: number;
};
