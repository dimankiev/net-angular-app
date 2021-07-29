import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-products-list',
  templateUrl: './products.component.html'
})
export class ProductsListComponent {
  public products: StockProduct[];

  constructor(http: HttpClient) {
    http.get<StockProduct[]>("https://localhost:5001/api/products/get").subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }
}

interface StockProduct {
  _id?: unknown;
  productId: number;
  name: string;
  category: string;
  size: string;
  price: number;
  stock: number;
}
