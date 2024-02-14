import { Component, Input } from '@angular/core';
import { Movie } from '../movie';

@Component({
  selector: 'app-prices',
  templateUrl: './prices.component.html',
  styleUrls: ['./prices.component.css']
})
export class PricesComponent {
  @Input() moviePrices: { [key: string]: any } = {}; 
  @Input() movieId = ''
}
