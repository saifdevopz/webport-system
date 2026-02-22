import { Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { form, FormField, minLength, required } from '@angular/forms/signals';
import { Button } from '../../shared/components/button';
import { RouterLink } from '@angular/router';
import { FormErrors } from '../../shared/components/form-errors';
import { Store } from '@ngrx/store';
import { authFeatures } from '../../shared/store/auth-feature';
import { authActions } from '../../shared/store/auth-actions';

@Component({
  selector: 'app-login',
  imports: [Button, RouterLink, FormField, FormsModule, FormErrors],
  templateUrl: './login.html',
  host: {
    class: 'min-h-screen flex items-center justify-center bg-slate-100 p-4',
  },
})
export class Login {
  loginModel = signal({
    username: 'johnd',
    password: 'm38rmF$',
  });

  loginForm = form(this.loginModel, (rootPath) => {
    required(rootPath.username, { message: 'Username is required' });
    required(rootPath.password, { message: 'Password is required' });
    minLength(rootPath.password, 6, { message: 'Password must be at least 6 characters long' });
  });

  private readonly store = inject(Store);
  protected readonly isLoading = toSignal(this.store.select(authFeatures.selectIsLoading));

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.loginForm().valid()) {
      this.store.dispatch(authActions.login(this.loginForm().value()));
    } else {
      console.log('Form is invalid');
    }
  }
}
