import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {FormBuilder} from "@angular/forms";

@Component({
  selector: 'app-customers-list',
  templateUrl: './customers.component.html'
})
export class CustomersListComponent {
  public customerList: Customer[];
  public isDialogOpened = false;

  newCustomerForm = this.formBuilder.group({
    name: '',
    address: ''
  });

  constructor(private http: HttpClient, private formBuilder: FormBuilder) {
    this.fetchCustomers();
  }

  fetchCustomers() {
    this.http.get<Customer[]>("https://localhost:5001/api/customers/get").subscribe(result => {
      this.customerList = result;
    }, error => console.error(error));
  }

  toggleCustomerDialog() {
    this.fetchCustomers();
    this.isDialogOpened = !this.isDialogOpened;
    this.newCustomerForm.reset();
  }
  async onSubmit(): Promise<void> {
    const response = await fetch("https://localhost:5001/api/customers/add", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(this.newCustomerForm.value)
    });
    const result = await response.json();
    alert(result.message);
    this.toggleCustomerDialog();
  }
}

interface Customer {
  _id?: unknown;
  name: string;
  address: string;
  totalCost: number;
  totalOrders: number;
}
