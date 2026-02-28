import { Component, inject } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { Store } from '@ngrx/store';
import { cartFeature } from './store/cart-feature';
import { cartActions } from './store/cart-actions';
import { Button } from '../../shared/components/button';
import { LucideAngularModule, Minus, Plus, ShoppingBag, Trash2 } from 'lucide-angular';

@Component({
  selector: 'app-cart',
  imports: [LucideAngularModule, RouterLink, Button, CurrencyPipe],
  templateUrl: './cart.html',
})
export class Cart {
  protected readonly icons = { ShoppingBag, Plus, Minus, Trash2 };
  private readonly store = inject(Store);

  protected readonly loading = toSignal(this.store.select(cartFeature.selectLoading));
  protected readonly items = toSignal(this.store.select(cartFeature.selectItems));
  protected readonly cartTotal = toSignal(this.store.select(cartFeature.selectCartTotal), {
    initialValue: 0,
  });
  protected readonly cartCount = toSignal(this.store.select(cartFeature.selectCartCount), {
    initialValue: 0,
  });

  protected onRemove(productId: number) {
    this.store.dispatch(cartActions.removeFromCart({ productId }));
  }

  protected onUpdateQuantity(productId: number, quantity: number) {
    this.store.dispatch(cartActions.updateQuantity({ productId, quantity }));
  }

  protected onClearCart() {
    this.store.dispatch(cartActions.clearCart());
  }
}
