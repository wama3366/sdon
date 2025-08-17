import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { Observable, of } from 'rxjs';
import { map, catchError  } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class UnAuthGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) {
    }

    canActivate(): Observable<boolean> {
        return this.auth.user().pipe(
            map(user => {
                const isLoggedIn = !!user?.sub;
                if (isLoggedIn) {
                    this.router.navigate(['/dashboard']);
                    // apply routing, do not allow original route.
                    return false;
                }
                // allow original route.
                return true;
            }),
            catchError(err => {
                if (err.status === 401) {
                    // Not logged in — allow access to landing
                    return of(true);
                }
                // Unexpected error — block route or rethrow
                return of(false);
            })
        );
    }
}