import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map } from 'rxjs';
import { authFeatures } from '../../shared/store/auth-feature';
import { NgToastService } from 'ng-angular-popup';

export const authGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);
  const toast = inject(NgToastService);

  return store.select(authFeatures.selectIsAuthenticated).pipe(
    map((isAuthenticated) => {
      if (!isAuthenticated) {
        toast.info('Please login to continue', 'INFO');
        return router.createUrlTree(['/login']);
      }
      return true;
    }),
  );
};
