import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_URL } from '../../app.config';
import { Product, WooProduct } from './product-model';
import { WoocommerceService } from '../../core/services/woocommerce.service';

@Injectable({
  providedIn: 'root',
})
export class ProductApi {
  private readonly baseApiUrl = inject(API_URL);
  private readonly http = inject(HttpClient);

  private baseUrl = 'https://store3.dev.aceonit.co.za';
  private consumerKey = 'ck_9d094f4988e804e3b7c57b269b9889ba3e1d488c';
  private consumerSecret = 'cs_f7d864290c544b643a4d7816318b60579b6385e7';

  products: WooProduct[] = [];
  loading = true;
  constructor(private wc: WoocommerceService) {}

  public getProducts() {
    return this.getWooCommerceProducts();
    //return this.http.get<Product[]>(`${this.baseApiUrl}/products`);
  }

  public getWooCommerceProducts() {
    return this.http.get<WooProduct[]>(`${this.baseUrl}/wp-json/wc/v3/products`, {
      params: {
        consumer_key: this.consumerKey,
        consumer_secret: this.consumerSecret,
        per_page: 100,
      },
    });
    // this.wc.getWooCommerceProducts().subscribe({
    //   next: (data) => {
    //     this.products = data;
    //     console.log(data);
    //     this.loading = false;
    //   },
    //   error: (err) => {
    //     console.error(err);
    //     this.loading = false;
    //   },
    // });
  }
}
