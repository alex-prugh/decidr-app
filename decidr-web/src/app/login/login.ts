import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';  // ngModel
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule], // ✅ no HttpClientModule needed
  templateUrl: './login.html'
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private router: Router, private http: HttpClient) {}

  login() {
    this.http.post<any>('https://localhost:5001/api/login/login', {
      username: this.username,
      password: this.password
    }).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);
        alert('Login successful!');
        this.router.navigate(['/home']);
      },
      error: () => alert('Invalid credentials')
    });
  }
}
