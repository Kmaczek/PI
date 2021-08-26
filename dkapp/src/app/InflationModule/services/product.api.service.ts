import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Product } from '../models/product';
import { PriceSerie, IPriceSerie } from '../models/priceSerie';
import { GrouppedProducts } from '../models/grouppedProducts';

@Injectable({
    providedIn: 'root',
})
export class ProductsService
{
    private allProductsUrl = `${environment.apiUrl}/product`;

    constructor(private httpClient: HttpClient ) 
    { }

    public GetProducts() : Observable<GrouppedProducts[]>
    {
        return this.httpClient.get<GrouppedProducts[]>(this.allProductsUrl);
    }

    public GetProductSeries(productId: number) : Observable<IPriceSerie[]>
    {
        var params = new HttpParams().set("productId", productId.toString());

        return this.httpClient.get<IPriceSerie[]>(this.allProductsUrl + '/series', { params });
    }
}