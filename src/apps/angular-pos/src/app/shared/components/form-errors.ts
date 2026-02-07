import { Component, computed, input } from '@angular/core';
import { FieldState } from '@angular/forms/signals';

@Component({
  selector: 'app-form-errors',
  template: `
    @if(shouldShowError()) { @for(error of control().errors(); track error.kind) {
    <small class="text-sm block text-red-500 mt-1">{{ error.message }}</small>
    } }
  `,
})
export class FormErrors {
  readonly control = input.required<FieldState<unknown>>();

  protected readonly shouldShowError = computed(() => {
    const field = this.control();
    return !field.valid() && field.touched();
  });
}
