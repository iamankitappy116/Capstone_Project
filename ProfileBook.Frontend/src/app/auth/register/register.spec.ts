import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Register } from './register';
import { Auth } from '../../core/services/auth';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('Register Component', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let authService: any;
  let router: any;

  beforeEach(async () => {
    authService = {
      register: vi.fn()
    };
    router = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      declarations: [Register],
      imports: [FormsModule],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: Auth, useValue: authService },
        { provide: Router, useValue: router }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show error when passwords do not match', () => {
    component.registerData.password = 'password123';
    component.registerData.confirmPassword = 'different';

    component.onSubmit();

    expect(component.errorMessage).toBe('Passwords do not match');
    expect(authService.register).not.toHaveBeenCalled();
  });

  it('should call auth.register when passwords match', () => {
    authService.register.mockReturnValue(of({}));
    component.registerData.username = 'newuser';
    component.registerData.email = 'new@test.com';
    component.registerData.password = 'pass123';
    component.registerData.confirmPassword = 'pass123';

    component.onSubmit();

    expect(authService.register).toHaveBeenCalled();
  });

  it('should show success message on successful registration', () => {
    authService.register.mockReturnValue(of({}));
    component.registerData.password = 'pass123';
    component.registerData.confirmPassword = 'pass123';

    component.onSubmit();

    expect(component.successMessage).toBe('Registration successful! You can now log in.');
  });

  it('should show error message on registration failure', () => {
    authService.register.mockReturnValue(throwError(() => ({ error: 'Email already registered.' })));
    component.registerData.password = 'pass123';
    component.registerData.confirmPassword = 'pass123';

    component.onSubmit();

    expect(component.errorMessage).toBe('Email already registered.');
  });
});
