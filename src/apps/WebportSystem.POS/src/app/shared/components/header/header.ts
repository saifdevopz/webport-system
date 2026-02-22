import { Component, inject } from '@angular/core';
import { authActions } from '../../store/auth-actions';
import { createFeature, Store } from '@ngrx/store';

import { LucideAngularModule, LogOut, User, ShoppingCart, Router } from 'lucide-angular';
import { Button } from '../button';
import { RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { cartFeature } from '../../../pages/cart/store/cart-feature';

@Component({
  selector: 'app-header',
  imports: [Button, RouterLink, LucideAngularModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  protected readonly icons = { LogOut, User, ShoppingCart };
  private readonly store = inject(Store);
  protected readonly cartItemCount = toSignal(this.store.select(cartFeature.selectCartCount), {
    initialValue: 0,
  });

  protected logout() {
    this.store.dispatch(authActions.logout());
  }
}
