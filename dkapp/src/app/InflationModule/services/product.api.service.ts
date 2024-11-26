import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GroupedProducts } from '../models/grouppedProducts';
import { IPriceEntry } from '../models/priceEntry';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  private allProductsUrl = `${environment.apiUrl}/product`;

  constructor(private httpClient: HttpClient) {}

  public GetProducts(): Observable<GroupedProducts[]> {
    return this.httpClient.get<GroupedProducts[]>(this.allProductsUrl);
  }

  public GetProductSeries(productId: number): Observable<IPriceEntry[]> {
    const params = new HttpParams().set('productId', productId.toString());

    return this.httpClient.get<IPriceEntry[]>(this.allProductsUrl + '/series', { params });
  }
}
