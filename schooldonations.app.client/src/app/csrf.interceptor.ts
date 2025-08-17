import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class CsrfInterceptor implements HttpInterceptor {
    // TODO: Check what this does and if this is sufficient or not to achieve it.
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Only add to BFF/API calls
        if (req.url.startsWith('/bff') || req.url.startsWith('/api')) {
            req = req.clone({
                setHeaders: { 'X-CSRF': '1' },
                withCredentials: true
            });
        }
        return next.handle(req);
    }
}