import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { LucideAngularModule, Star } from 'lucide-angular';
import { Button } from '../../shared/components/button';
import { CurrencyPipe } from '@angular/common';
import { Product } from './product-model';

@Component({
  selector: 'app-product-card',
  imports: [Button, LucideAngularModule, CurrencyPipe],
  template: `
    <div class="relative overflow-hidden rounded-lg bg-white">
      <img
        [src]="product().image"
        [alt]="product().title"
        width="300"
        height="300"
        class="w-full h-48 object-contain p-4"
      />
      <span
        class="absolute top-2 left-2 px-2 py-1 text-xs font-medium bg-green-200 text-slate-700 rounded"
      >
        {{ product().category }}
      </span>
    </div>

    <div class="p-4 space-y-3">
      <h3 class="font-semibold text-slate-900 line-clamp-2 min-h-12">
        {{ product().title }}
      </h3>

      <div class="flex items-center gap-2">
        <div class="flex items-center gap-1 text-amber-500">
          <lucide-icon [img]="icons.Star" class="size-4 fill-current" />
          <span class="text-sm font-medium text-slate-700">{{ product().rating.rate }}</span>
        </div>
        <span class="text-xs text-slate-500">({{ product().rating.count }} reviews)</span>
      </div>

      <div class="flex items-center justify-between pt-2">
        <span class="text-xl font-bold text-slate-900">{{ product().price | currency: 'R' }}</span>
        <button appButton size="sm" type="button" (click)="addToCart.emit(product())">
          Add to Cart
        </button>
      </div>
    </div>
  `,
  host: {
    class: 'block bg-white rounded-xl shadow-md hover:shadow-lg transition-shadow overflow-hidden',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductCard {
  readonly icons = { Star };
  readonly product = input.required<Product>();
  readonly addToCart = output<Product>();
}
