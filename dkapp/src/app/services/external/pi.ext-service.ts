import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from '../../../environments/environment'

@Injectable({
  providedIn: 'root',
})
export class PiExtService {
  constructor(private httpClient: HttpClient) {}

  public GetWeatherForecast() {
    this.httpClient.get(environment.apiUrl + '/weatherforecast').subscribe((x) => console.log(x))
  }
}
