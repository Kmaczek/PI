import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { FlatSeries } from '../models/flatSerie';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FlatService {
  private flatSeriesUrl = `${environment.apiUrl}/flat/series`;

  constructor(private httpClient: HttpClient) {}

  public GetFlatsSeries(): Observable<FlatSeries[]> {
    return this.httpClient.get<FlatSeries[]>(this.flatSeriesUrl);
  }
}
