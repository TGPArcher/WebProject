import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpInterceptor, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthorizeInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService, private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authService.isLoggedIn()) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Basic ${this.authService.getUserAuthData()}`
                }
            });
        } else {
            if (this.router.url !== '/login') {
                this.router.navigate(['/login']);
            }
        }

        return next.handle(req);
    }
}
