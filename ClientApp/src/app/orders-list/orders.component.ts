import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {
  public orders: Order[];

  constructor(http: HttpClient) {
    http.get<Order[]>("https://localhost:5001/api/orders/get").subscribe(result => {
      this.orders = result;
    }, error => console.error(error));
  }
}

interface Order {
  _id?: unknown;
  number: number;
  customerName: string;
  customerAddress: string;
  products: unknown;
  total: number;
  status: string;
}
