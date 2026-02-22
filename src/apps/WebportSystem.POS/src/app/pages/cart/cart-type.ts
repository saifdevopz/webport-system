import { WooProduct } from '../products/product-model';

export type CartItem = {
  product: WooProduct;
  quantity: number;
};
