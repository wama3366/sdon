import { Component } from '@angular/core';

import { AuthService } from '../auth/auth.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './unauth-dashboard.component.html',
    standalone: true,
})

export class UnAuthDashboardComponent {
    constructor(private authService: AuthService) {}

    login() {
        this.authService.login();
    }
}

