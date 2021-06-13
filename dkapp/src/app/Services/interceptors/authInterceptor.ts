import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { IdentityService } from 'src/app/services/external/identity.ext.service';

@Injectable({
    providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor 
{
    constructor(
        public identityService: IdentityService) 
    {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const idToken = localStorage.getItem("id_token");

        if (idToken) {
            const cloned = req.clone({
                headers: req.headers.set("Authorization", "Bearer " + idToken)
            });

            return next.handle(cloned);
        }
        else {
            return next.handle(req);
        }
    }
}