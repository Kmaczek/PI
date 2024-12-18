import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GroupedProducts } from '../models/groupedProducts';
import { IPriceEntry } from '../models/priceEntry';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root',
})
export class ProductApiService {
  private productsUrl = `${environment.apiUrl}/product`;

  constructor(private httpClient: HttpClient) {}

  public getProducts(): Observable<GroupedProducts[]> {
    return this.httpClient.get<GroupedProducts[]>(this.productsUrl);
  }

  public getProduct(id: number): Observable<Product> {
    return this.httpClient.get<Product>(this.productsUrl + `/${id}`);
  }

  public getProductSeries(priceDetailsId: number): Observable<IPriceEntry[]> {
    const params = new HttpParams().set('priceDetailsId', priceDetailsId.toString());

    return this.httpClient.get<IPriceEntry[]>(this.productsUrl + '/series', {
      params,
    });
  }
}
