import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { provideState } from '@ngrx/store';
import { cartFeature } from './pages/cart/store/cart-feature';
import { provideEffects } from '@ngrx/effects';
import { profileFeature } from './pages/profile/store/profile-feature';
import { productFeature } from './pages/products/store/product-feature';

import * as productEffect from './pages/products/store/product-effect';
import * as cartEffects from './pages/cart/store/cart-effect';
import * as profileEffect from './pages/profile/store/profile-effect';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login').then((m) => m.Login),
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register/register').then((m) => m.Register),
  },

  {
    path: '',
    loadComponent: () => import('./pages/main-layout/main-layout').then((m) => m.MainLayout),
    canActivate: [authGuard],
    providers: [provideState(cartFeature), provideEffects(cartEffects)],
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'products',
      },
      {
        path: 'products',
        loadComponent: () => import('./pages/products/products').then((m) => m.Products),
        providers: [provideState(productFeature), provideEffects(productEffect)],
      },
      {
        path: 'profile',
        loadComponent: () => import('./pages/profile/profile').then((m) => m.Profile),
        providers: [provideState(profileFeature), provideEffects(profileEffect)],
      },
      {
        path: 'cart',
        loadComponent: () => import('./pages/cart/cart').then((m) => m.Cart),
      },
    ],
  },
];
