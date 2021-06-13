import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { tap, shareReplay } from 'rxjs/operators';
import * as moment from "moment";
import jwt_decode from 'jwt-decode';

@Injectable({
    providedIn: 'root',
})
export class IdentityService
{
    private endpoint = environment.apiUrl + '/auth'

    constructor(
        private httpClient: HttpClient)
    {}

    public GetWeatherForecast()
    {
        this.httpClient.get(environment.apiUrl + '/weatherforecast')
            .subscribe(x => console.log(x));
    }

    public login(username: string, password: string): Observable<any>
    {
        return this.httpClient.post(this.endpoint + '/login', {username, password})
            .pipe(
                tap(res => this.setSession(res)), 
                shareReplay());
    }

    private setSession(authResult) {
        var decoded = jwt_decode<any>(authResult.token); 
        console.log(decoded);
        const expiresAt = moment.unix(decoded.exp);

        localStorage.setItem('email', JSON.stringify(decoded.email));
        localStorage.setItem('unique_name', JSON.stringify(decoded.unique_name));
        localStorage.setItem("expires_at", JSON.stringify(expiresAt) );
        localStorage.setItem('given_name', JSON.stringify(decoded.given_name));
        localStorage.setItem('role', JSON.stringify(decoded.role));
    }

    logout()
    {
        localStorage.removeItem("id_token");
        localStorage.removeItem("expires_at");
    }

    isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    isLoggedOut() {
        return !this.isLoggedIn();
    }

    getExpiration() {
        const expiration = localStorage.getItem("expires_at");
        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }
}