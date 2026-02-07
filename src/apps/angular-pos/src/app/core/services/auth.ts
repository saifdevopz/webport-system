import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';

export type LoginRequest = {
  username: string;
  password: string;
};

export type LoginResponse = {
  token: string;
};

export type RegisterRequest = {
  id: number;
  username: string;
  email: string;
  password: string;
};

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private readonly baseApiUrl = inject(API_URL);
  private readonly http = inject(HttpClient);

  login(request: LoginRequest) {
    const url = `${this.baseApiUrl}/auth/login`;
    return this.http.post<LoginResponse>(url, request);
  }

  register(request: RegisterRequest) {
    const url = `${this.baseApiUrl}/users`;
    return this.http.post<RegisterRequest>(url, request);
  }
}
