import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserProfile } from './models';

@Injectable({
  providedIn: 'root',
})
export class UserApiService {
  private endpoint = environment.apiUrl + '/user';

  constructor(private httpClient: HttpClient) {}

  public getUser(): Observable<UserProfile> {
    return this.httpClient.get<UserProfile>(this.endpoint);
  }
}
