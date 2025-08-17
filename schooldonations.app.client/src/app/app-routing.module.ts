import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './auth/auth.guard';
import { UnAuthGuard} from "./auth/unauth.guard";
import { SignupComponent } from './auth/signup/signup.component';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UnAuthDashboardComponent} from "./dashboard/unauth-dashboard.component";
import { CustomerComponent } from './customers/customer.component';

const routes: Routes = [
  { path: '', component: UnAuthDashboardComponent, canActivate: [UnAuthGuard] },
  { path: 'signup', component: SignupComponent },
  { path: 'login',  component: LoginComponent },
  { path: 'logout', component: LogoutComponent },

  { path: 'dashboard',  component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'customers',   component: CustomerComponent, canActivate: [AuthGuard] },

    // redirects
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }