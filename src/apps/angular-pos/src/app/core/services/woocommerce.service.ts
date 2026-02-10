import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root',
})
export class WoocommerceService {
  private baseUrl = 'https://azaaessentials.co.za';
  private consumerKey = 'ck_49e6e7dab5520e4dc0da92b210e0e2aa3ca9475b';
  private consumerSecret = 'cs_ba1a9139e686d20e1838ea9bafa9140a3153382e';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/wp-json/wc/v3/products`, {
      params: {
        consumer_key: this.consumerKey,
        consumer_secret: this.consumerSecret,
        per_page: 100,
      },
    });
  }
}
