import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import jwt_decode from 'jwt-decode'
import * as moment from 'moment'
import { BehaviorSubject, Observable } from 'rxjs'
import { map } from 'rxjs/operators'
import { environment } from '../../../environments/environment'
import { LoginResponse, TokenModel } from './models'

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  public isLoggedIn: boolean = false;

  private endpoint = environment.apiUrl + '/auth';
  private expiresAt: moment.Moment;

  constructor(private httpClient: HttpClient) {
    this.expiresAt = moment(localStorage.getItem('expires_at'));
    this.verifyToken();
    this.isLoggedInSubject.next(this.isLoggedIn);
  }

  public GetWeatherForecast() {
    this.httpClient.get(environment.apiUrl + '/weatherforecast').subscribe((x) => console.log(x))
  }

  public login(username: string, password: string): Observable<boolean> {
    return this.httpClient
      .post<LoginResponse>(this.endpoint + '/login', { username, password })
      .pipe(map(response => {
        this.setSession(response);
        this.verifyToken();
        this.isLoggedInSubject.next(this.isLoggedIn);
        return this.isLoggedIn;
      }));
  }

  private setSession(loginResponse: LoginResponse): void {
    const decoded = jwt_decode<TokenModel>(loginResponse.token);
    console.log(decoded);
    const expiresAt = moment.unix(decoded.exp);

    localStorage.setItem('email', JSON.stringify(decoded.email));
    localStorage.setItem('unique_name', JSON.stringify(decoded.unique_name));
    localStorage.setItem('expires_at', expiresAt.toISOString());
    localStorage.setItem('given_name', JSON.stringify(decoded.given_name));
    localStorage.setItem('role', JSON.stringify(decoded.role));

    localStorage.setItem('id_token', loginResponse.token);
  }

  logout() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');

    this.isLoggedIn = false;
    this.isLoggedInSubject.next(this.isLoggedIn);
  }

  private verifyToken(): boolean {
    this.isLoggedIn = moment().isBefore(this.expiresAt);

    return this.isLoggedIn;
  }
}
