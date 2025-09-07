import { Routes } from '@angular/router';
import { LoginComponent } from './login/login';
import { RegisterComponent } from './register/register';
import { HomeComponent } from './home/home';
import { SetDetailComponent } from './set-detail/set-detail';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  { path: 'sets/:id', component: SetDetailComponent },
  { path: '**', redirectTo: 'home' },
];
