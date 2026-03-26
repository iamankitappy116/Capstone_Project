import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Login } from './login';
import { Auth } from '../../core/services/auth';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authService: any;
  let router: any;

  beforeEach(async () => {
    authService = {
      login: vi.fn()
    };
    router = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      declarations: [Login],
      imports: [ReactiveFormsModule],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: Auth, useValue: authService },
        { provide: Router, useValue: router }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with an invalid empty form', () => {
    expect(component.loginForm.valid).toBe(false);
  });

  it('should show error when form is submitted empty', () => {
    component.onSubmit();
    expect(component.errorMessage).toBe('Please fill in all fields.');
    expect(authService.login).not.toHaveBeenCalled();
  });

  it('should call auth.login with form values on valid submit', () => {
    authService.login.mockReturnValue(of({ token: 'abc', role: 'User', userId: 1 }));
    component.loginForm.setValue({ username: 'testuser', password: 'pass123' });

    component.onSubmit();

    expect(authService.login).toHaveBeenCalledWith({ username: 'testuser', password: 'pass123' });
  });

  it('should navigate to /admin-dashboard when role is Admin', () => {
    authService.login.mockReturnValue(of({ token: 'abc', role: 'Admin', userId: 1 }));
    component.loginForm.setValue({ username: 'admin', password: 'admin123' });

    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/admin-dashboard']);
  });

  it('should navigate to /dashboard/feed when role is User', () => {
    authService.login.mockReturnValue(of({ token: 'abc', role: 'User', userId: 2 }));
    component.loginForm.setValue({ username: 'user1', password: 'pass123' });

    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/dashboard/feed']);
  });

  it('should set errorMessage on 400 error', () => {
    authService.login.mockReturnValue(throwError(() => ({ status: 400, error: 'Wrong password.' })));
    component.loginForm.setValue({ username: 'user1', password: 'wrongpass' });

    component.onSubmit();

    expect(component.errorMessage).toBe('Wrong password.');
  });

  it('should set server error message on status 0', () => {
    authService.login.mockReturnValue(throwError(() => ({ status: 0 })));
    component.loginForm.setValue({ username: 'user1', password: 'pass123' });

    component.onSubmit();

    expect(component.errorMessage).toBe('Cannot connect to server. Please try again later.');
  });
});
