import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.html'
})
export class RegisterComponent {
  username = '';
  password = '';
  name = '';
  email = '';
  validationErrors: any = {};

  constructor(private http: HttpClient, private router: Router) {}

  register() {
    this.validationErrors = {};
    this.http.post<any>('https://localhost:5001/api/auth/register', {
      username: this.username,
      password: this.password,
      name: this.name,
      email: this.email,
    }).subscribe({
      next: () => {
        alert('Registration successful! Please log in.');
        this.router.navigate(['/login']);
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === 400 && err.error && err.error.errors) {
          this.validationErrors = err.error.errors;
        } else {
          alert(err.error.title || 'Registration failed');
        }
      }
    });
  }
}
