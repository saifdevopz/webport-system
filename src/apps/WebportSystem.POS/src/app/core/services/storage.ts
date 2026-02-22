import { Injectable } from '@angular/core';
import { extractToken } from '../../shared/utils/extractToken';

@Injectable({
  providedIn: 'root',
})
export class Storage {
  setItem(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  getItem(key: string): string | null {
    return localStorage.getItem(key);
  }

  removeItem(key: string): void {
    localStorage.removeItem(key);
  }

  clear(): void {
    localStorage.clear();
  }

  getUserId(): number | null {
    const token = this.getItem('ngrxstore_token');
    if (!token) {
      return null;
    }
    const payload = extractToken(token);
    return payload ? payload.sub : null;
  }
}
