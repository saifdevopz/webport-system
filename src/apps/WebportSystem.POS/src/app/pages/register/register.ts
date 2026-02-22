import { Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormField, form } from '@angular/forms/signals';
import { Button } from '../../shared/components/button';
import { RouterLink } from '@angular/router';
import { FormErrors } from '../../shared/components/form-errors';
import { registerSchema } from './register-schema';
import { Store } from '@ngrx/store';
import { authActions } from '../../shared/store/auth-actions';
import { authFeatures } from '../../shared/store/auth-feature';

@Component({
  selector: 'app-register',
  imports: [Button, RouterLink, FormField, FormErrors],
  templateUrl: './register.html',
  host: {
    class: 'min-h-screen flex items-center justify-center bg-slate-100 p-4',
  },
})
export class Register {
  registerModel = signal({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  registerForm = form(this.registerModel, registerSchema);
  private readonly store = inject(Store);
  protected readonly isLoading = toSignal(this.store.select(authFeatures.selectIsLoading));

  onSubmit(event: Event) {
    event.preventDefault();
    const id = Date.now();
    const { confirmPassword, ...rest } = this.registerForm().value();
    const registerRequest = { id, ...rest };
    this.store.dispatch(authActions.register(registerRequest));
  }
}
