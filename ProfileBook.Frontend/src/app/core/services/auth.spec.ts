import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Auth } from './auth';

describe('Auth Service', () => {
  let service: Auth;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [Auth]
    });
    service = TestBed.inject(Auth);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should store token on successful login', () => {
    const mockResponse = {
      token: 'fake-jwt-token',
      username: 'testadmin',
      role: 'Admin',
      userId: 1
    };

    service.login({ username: 'testadmin', password: 'password' }).subscribe(res => {
      expect(res.token).toBe('fake-jwt-token');
      expect(localStorage.getItem('token')).toBe('fake-jwt-token');
      expect(localStorage.getItem('role')).toBe('Admin');
    });

    const req = httpMock.expectOne('http://localhost:5134/api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should return true for isLoggedIn when token exists', () => {
    localStorage.setItem('token', 'exists');
    expect(service.isLoggedIn()).toBe(true);
  });
});
