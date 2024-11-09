import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ProductSearchComponent } from './product-search/product-search.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ProductSearchComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'product-search-app';
}
