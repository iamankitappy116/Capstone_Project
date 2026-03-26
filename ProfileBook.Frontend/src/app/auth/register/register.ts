import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth';
@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  registerData = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };  

  errorMessage: string = '';
  successMessage: string = '';

  constructor(private auth: Auth, private router: Router) {}

  goToNextField(event: any, nextInput: any) {
    event.preventDefault();
    if (nextInput && typeof nextInput.focus === 'function') {
      nextInput.focus();
    }
  }

  onSubmit(){
    this.errorMessage = '';
    this.successMessage = '';
    if(this.registerData.password !== this.registerData.confirmPassword){
      this.errorMessage = 'Passwords do not match';
      return;
    }
    this.auth.register(this.registerData).subscribe({
      next: (res: any) => {
        this.successMessage = 'Registration successful! You can now log in.';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err: any) => {
        this.errorMessage = err.error || 'Registration failed.';
      },
    });
  }
}
