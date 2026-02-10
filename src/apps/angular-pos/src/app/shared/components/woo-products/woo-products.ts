import { Component } from '@angular/core';
import { WoocommerceService } from '../../../core/services/woocommerce.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-woo-products',
  imports: [CommonModule],
  templateUrl: './woo-products.html',
  styleUrl: './woo-products.css',
})
export class WooProducts {
  products: any[] = [];
  loading = true;
  constructor(private wc: WoocommerceService) {}

  ngOnInit() {
    this.wc.getProducts().subscribe({
      next: (data) => {
        this.products = data;
        console.log(data);
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }
}
