import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../auth/auth.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    standalone: true,
})

export class DashboardComponent {
    constructor(private router: Router,
                private authService: AuthService) {}

    goToCustomers() {
        this.router.navigate(['/customers']);
    }

    logout() {
        this.authService.logout();
    }
}
