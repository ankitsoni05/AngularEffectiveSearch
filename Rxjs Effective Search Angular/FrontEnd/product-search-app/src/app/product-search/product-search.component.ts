import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { count, debounceTime, distinctUntilChanged, map, Observable, switchMap } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AsyncPipe, JsonPipe, NgFor, NgIf } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ProductData } from './Models/productData';

@Component({
  selector: 'app-product-search',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatIconModule, ReactiveFormsModule, AsyncPipe, MatAutocompleteModule, HttpClientModule, NgFor, NgIf, JsonPipe],
  templateUrl: './product-search.component.html',
  styleUrl: './product-search.component.css'
})
export class ProductSearchComponent {

  searchControl = new FormControl();
  filteredProducts: ProductData | undefined;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(value => this.searchProducts(value))
    ).subscribe(data => {
      this.filteredProducts = data;
    });
  }

  searchProducts(query: string): Observable<ProductData> {
    const params = new HttpParams().set('searchText', query);
    return this.http.get<ProductData>('https://localhost:7151/products', { params });
  }

}
