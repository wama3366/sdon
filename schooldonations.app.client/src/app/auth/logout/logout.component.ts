import { Component } from '@angular/core';

import { AuthService} from '../auth.service';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html',
    standalone: true,
})

export class LogoutComponent {
    constructor(private auth: AuthService) {
        this.auth.logout();
    }
}
