import { OidcClient, UserManager } from "oidc-client";
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class IdentityService
{
    private mgr: UserManager;

    constructor(
        private httpClient: HttpClient)
    {
        var config = {
            authority: "http://localhost:5000",
            client_id: "js",
            redirect_uri: "http://localhost:4200/callback",
            response_type: "code",
            scope: "openid profile api1",
            post_logout_redirect_uri: "http://localhost:4200",
        };
        this.mgr = new UserManager(config);
        this.mgr.getUser().then(function (user)
        {
            if (user) {
                console.log("User logged in", user.profile);
            }
            else {
                console.log("User not logged in");
            }
        });
    }

    public GetWeatherForecast()
    {
        this.httpClient.get(environment.apiUrl + '/weatherforecast')
            .subscribe(x => console.log(x));
    }

    public login()
    {
        this.mgr.signinRedirect();
    }

    public api()
    {
        this.mgr.getUser().then(user =>
        {
            var url = "http://localhost:5001/identity";

            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function ()
            {
                console.log(xhr.status, JSON.parse(xhr.responseText));
            }
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        });
    }

    public logout()
    {
        this.mgr.signoutRedirect();
    }
}