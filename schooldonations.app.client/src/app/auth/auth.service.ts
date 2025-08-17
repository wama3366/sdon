import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

import {AuthUser} from './authuser.dto';
import {Claim} from './claim.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
    constructor(private http: HttpClient) {
    }

    login(returnUrl = '/'): void {
        window.location.href = `/bff/login?returnUrl=${encodeURIComponent(returnUrl)}`;
    }

    logout(): void {
        this.user()
            .subscribe(user => {
                window.location.href = user['bff:logout_url'] + `&returnUrl=${encodeURIComponent('/')}`;
            });
    }

    user(): Observable<AuthUser> {
        return this.http.get<Claim[]>('/bff/user').pipe(
            map(claims => {
                const user: Record<string, string> = {};
                for (const claim of claims) {
                    user[claim.type] = claim.value;
                }
                return user as AuthUser;
            })
        );
    }
}