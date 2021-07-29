import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-products-list',
  templateUrl: './products.component.html'
})
export class ProductsListComponent {
  public products: StockProduct[];
  public isDialogOpened = false;

  checkoutForm = this.formBuilder.group({
    name: '',
    category: '',
    quantity: '',
    price: '',
    description: ''
  });

  constructor(private http: HttpClient, private formBuilder: FormBuilder) {
    this.fetchProducts();
  }

  fetchProducts() {
    this.http.get<StockProduct[]>("https://localhost:5001/api/products/get").subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }
  toggleProductDialog() {
    this.fetchProducts();
    this.isDialogOpened = !this.isDialogOpened;
    this.checkoutForm.reset();
  }
  async onSubmit(): Promise<void> {
    const response = await fetch("https://localhost:5001/api/products/add", {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(this.checkoutForm.value) // body data type must match "Content-Type" header
    });
    const result = await response.json();
    alert(result.message);
    this.toggleProductDialog();
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
