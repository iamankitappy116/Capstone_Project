import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DashboardService } from './dashboard';

describe('DashboardService', () => {
  let service: DashboardService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DashboardService]
    });
    service = TestBed.inject(DashboardService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no outstanding requests
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get stats', () => {
    const mockStats = { totalUsers: 10, pendingPosts: 5 };
    service.getStats().subscribe(stats => {
      expect(stats).toEqual(mockStats);
    });

    const req = httpMock.expectOne('http://localhost:5134/api/dashboard/stats');
    expect(req.request.method).toBe('GET');
    req.flush(mockStats);
  });

  it('should create a new user', () => {
    const newUser = { username: 'test', email: 'test@test.com', password: '123' };
    service.createUser(newUser).subscribe(user => {
      expect(user).toBeTruthy();
    });

    const req = httpMock.expectOne('http://localhost:5134/api/user');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newUser);
    req.flush({ id: 1, ...newUser });
  });

  it('should update an existing user', () => {
    const updatedUser = { username: 'updated', email: 'upd@test.com' };
    const userId = 1;
    service.updateUser(userId, updatedUser).subscribe(user => {
      expect(user).toBeTruthy();
    });

    const req = httpMock.expectOne(`http://localhost:5134/api/user/${userId}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedUser);
    req.flush({ id: userId, ...updatedUser });
  });

  it('should delete a user', () => {
    const userId = 1;
    service.deleteUser(userId).subscribe(response => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne(`http://localhost:5134/api/user/${userId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
