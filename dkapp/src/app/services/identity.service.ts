import { Injectable, signal } from '@angular/core'
import jwt_decode from 'jwt-decode'
import { Observable } from 'rxjs'
import { map } from 'rxjs/operators'
import { IdentityApiService } from './api/identity.api.service'
import { LoginResponse, TokenModel } from './api/models'
import moment from 'moment';
import { Store } from '@ngrx/store'
import * as UserActions from '../state/user-actions';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private expiresAt: moment.Moment;
  public isLoggedIn = signal(false);

  constructor(private identityApi: IdentityApiService, private store: Store) {
    this.expiresAt = moment(localStorage.getItem('expires_at'));
    this.verifyToken();
  }

  login(username: string, password: string): Observable<boolean> {
    return this.identityApi.login(username, password)
      .pipe(map(response => {
        this.setSession(response);
        this.verifyToken();
        this.store.dispatch(UserActions.loadUser());
        return this.isLoggedIn();
      }));
  }

  logout() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');

    this.isLoggedIn.set(false);

    this.store.dispatch(UserActions.clearUser());
  }

  private setSession(loginResponse: LoginResponse): void {
    const decoded = jwt_decode<TokenModel>(loginResponse.token);
    this.expiresAt = moment.unix(decoded.exp);

    localStorage.setItem('email', JSON.stringify(decoded.email));
    localStorage.setItem('unique_name', JSON.stringify(decoded.unique_name));
    localStorage.setItem('expires_at', this.expiresAt.toISOString());
    localStorage.setItem('given_name', JSON.stringify(decoded.given_name));
    localStorage.setItem('role', JSON.stringify(decoded.role));

    localStorage.setItem('id_token', loginResponse.token);
  }

  private verifyToken(): void {
    this.isLoggedIn.set(moment().isBefore(this.expiresAt));
  }
}
