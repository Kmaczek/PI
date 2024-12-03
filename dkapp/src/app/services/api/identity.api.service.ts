import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginResponse } from './models';

@Injectable({
  providedIn: 'root',
})
export class IdentityApiService {
  private endpoint = environment.apiUrl + '/auth';

  constructor(private httpClient: HttpClient) {}

  public GetWeatherForecast() {
    this.httpClient.get(environment.apiUrl + '/weatherforecast').subscribe((x) => console.log(x));
  }

  public login(email: string, password: string): Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this.endpoint + '/login', { email: email, password });
  }
}
