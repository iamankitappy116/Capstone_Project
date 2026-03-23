import { Component } from '@angular/core';
import { Auth } from '../../core/services/auth';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  loginForm: FormGroup;

  errorMessage: string = '';
  formSubmitted: boolean = false;
  isLoading: boolean = false;
  
  constructor(private fb: FormBuilder, private auth: Auth, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  
  goToPassword(event: any, input: any) {
    event.preventDefault();
    
    if (input && typeof input.focus === 'function') {
      input.focus();
    } else {
      const passwordEl = document.getElementById('password');
      if (passwordEl) {
        passwordEl.focus();
      }
    }
  }

  onSubmit(){
    this.formSubmitted = true;
    this.errorMessage = '';

    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    this.isLoading = true;
    this.auth.login(this.loginForm.value).subscribe({
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
  get f() {
    return this.loginForm.controls;
  }
}
