import { Component, inject, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Store } from '@ngrx/store';
import { productFeature } from './store/product-feature';
import { productActions } from './store/product-actions';
import { Product } from './product-type';
import { ProductCard } from './product-card';
import { FormsModule } from '@angular/forms';
import { cartActions } from '../cart/store/cart-actions';

@Component({
  selector: 'app-products',
  imports: [ProductCard, FormsModule],
  templateUrl: './products.html',
})
export class Products implements OnInit {
  private readonly store = inject(Store);
  protected readonly products = toSignal(this.store.select(productFeature.selectFilteredProducts));
  protected readonly loading = toSignal(this.store.select(productFeature.selectLoading));

  protected searchQuery = signal('');

  protected onSearch(query: string): void {
    this.store.dispatch(productActions.search({ searchQuery: query }));
  }

  ngOnInit(): void {
    this.store.dispatch(productActions.load());
  }

  protected onAddToCart(product: Product): void {
    this.store.dispatch(cartActions.addToCart({ product }));
  }
}
