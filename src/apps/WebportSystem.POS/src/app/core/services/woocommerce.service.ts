import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Product } from '../../pages/products/product-model';

@Injectable({
  providedIn: 'root',
})
export class WoocommerceService {
  private baseUrl = 'https://store3.dev.aceonit.co.za';
  private consumerKey = 'ck_9d094f4988e804e3b7c57b269b9889ba3e1d488c';
  private consumerSecret = 'cs_f7d864290c544b643a4d7816318b60579b6385e7';

  constructor(private http: HttpClient) {}

  getWooCommerceProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}/wp-json/wc/v3/products`, {
      params: {
        consumer_key: this.consumerKey,
        consumer_secret: this.consumerSecret,
        per_page: 1,
      },
    });
  }
}
