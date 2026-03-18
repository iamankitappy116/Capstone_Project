import { Component } from '@angular/core';
import { Auth } from '../../core/services/auth';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginData = {
    username: '',
    password: '',
  };

  errorMessage: string = '';
  formSubmitted: boolean = false;
  isLoading: boolean = false;

  constructor(private auth: Auth, private router: Router) {}

  onSubmit(){
    this.formSubmitted = true;
    this.errorMessage = '';

    if (!this.loginData.username.trim() || !this.loginData.password.trim()) {
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    this.isLoading = true;
    this.auth.login(this.loginData).subscribe({
      next: (res: any) => {
        this.isLoading = false;
        const role = res.role || localStorage.getItem('role');
        if (role === 'Admin') {
          this.router.navigate(['/admin-dashboard']);
        } else {
          this.router.navigate(['/dashboard/feed']);
        }
      },
      error: (err: any) => {
        this.isLoading = false;
        if (err.status === 400) {
          this.errorMessage = err.error || 'Incorrect username or password. Please try again.';
        } else if (err.status === 0) {
          this.errorMessage = 'Cannot connect to server. Please try again later.';
        } else {
          this.errorMessage = 'An unexpected error occurred. Please try again.';
        }
      },
    });
  }
}
