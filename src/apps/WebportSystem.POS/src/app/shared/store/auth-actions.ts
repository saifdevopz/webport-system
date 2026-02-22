import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { RegisterRequest } from '../../core/services/auth';

export const authActions = createActionGroup({
  source: 'Auth',
  events: {
    login: props<{ username: string; password: string }>(),
    loginSuccess: props<{ token: string; userId: number | null }>(),
    loginFailure: props<{ error: string }>(),

    register: props<RegisterRequest>(),
    registerSuccess: emptyProps(),
    registerFailure: props<{ error: string }>(),

    logout: emptyProps(),
    logoutSuccess: emptyProps(),
  },
});
