import { Component } from '@angular/core';

@Component({
    selector: 'app-logout',
    templateUrl: './signup.component.html',
    standalone: true,
})

export class SignupComponent {
    register(): void {
        const returnUrl = encodeURIComponent(window.location.origin + '/login');
        window.location.href = '/account/register?returnUrl=' + returnUrl;
    }
}
