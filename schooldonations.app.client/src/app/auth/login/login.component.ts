import { Component, OnInit } from '@angular/core';

import { AuthService} from '../auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    standalone: true,
})

export class LoginComponent implements OnInit {
    constructor(private auth: AuthService) {
    }

    ngOnInit() {
        this.auth.user().subscribe(user => {
            if (!user?.sub) {
                // not logged in → trigger login
                this.auth.login();
            } else {
                // already logged in (possibly redirected back) → go to dashboard
                window.location.href = '/dashboard';
            }
        });
    }
}