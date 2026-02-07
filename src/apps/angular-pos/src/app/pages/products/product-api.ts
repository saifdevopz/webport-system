import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { Product } from './product-type';

@Injectable({
  providedIn: 'root',
})
export class ProductApi {
  private readonly baseApiUrl = inject(API_URL);
  private readonly http = inject(HttpClient);

  public getProducts() {
    return this.http.get<Product[]>(`${this.baseApiUrl}/products`);
  }
}
