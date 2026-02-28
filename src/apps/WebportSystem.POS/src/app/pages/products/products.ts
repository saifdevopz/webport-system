import { Component, inject, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Store } from '@ngrx/store';
import { productFeature } from './store/product-feature';
import { productActions } from './store/product-actions';
import { ProductCard } from './product-card';
import { FormsModule } from '@angular/forms';
import { cartActions } from '../cart/store/cart-actions';
import { Product } from './product-model';

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

  ngOnInit(): void {
    console.log('Product Page - ngOnInit');
    this.store.dispatch(productActions.load());
  }

  protected onSearch(query: string): void {
    this.store.dispatch(productActions.search({ searchQuery: query }));
  }

  protected onAddToCart(product: Product): void {
    this.store.dispatch(cartActions.addToCart({ product }));
  }
}
