import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-customers-list',
  templateUrl: './customers.component.html'
})
export class CustomersListComponent {
  public customerList: Customer[];

  constructor(http: HttpClient) {
    http.get<Customer[]>("https://localhost:5001/api/customers/get").subscribe(result => {
      this.customerList = result;
    }, error => console.error(error));
  }
}

interface Customer {
  _id?: unknown;
  name: string;
  address: string;
  totalCost: number;
  totalOrders: number;
}
