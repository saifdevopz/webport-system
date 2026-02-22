import { minLength, required, schema, validate } from '@angular/forms/signals';

export type RegisterModel = {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
};

export const registerSchema = schema<RegisterModel>((rootPath) => {
  required(rootPath.username, { message: 'Username is required' });
  required(rootPath.email, { message: 'Email is required' });
  required(rootPath.password, { message: 'Password is required' });
  minLength(rootPath.password, 6, { message: 'Password must be at least 6 characters long' });
  required(rootPath.confirmPassword, { message: 'Confirm Password is required' });

  validate(rootPath.confirmPassword, ({ value, valueOf }) => {
    const password = valueOf(rootPath.password);
    const confirmPassword = value();

    if (!password) {
      return null;
    }

    if (password !== confirmPassword) {
      return {
        kind: 'passwordMismatch',
        message: 'Passwords do not match',
      };
    }

    return null;
  });
});
