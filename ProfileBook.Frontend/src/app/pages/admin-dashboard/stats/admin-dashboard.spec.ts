import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminDashboardComponent } from './admin-dashboard';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DashboardService } from '../../../core/services/dashboard';
import { SignalRService } from '../../../core/services/signalr';
import { of, Subject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('AdminDashboardComponent', () => {
  let component: AdminDashboardComponent;
  let fixture: ComponentFixture<AdminDashboardComponent>;
  let dashboardService: any;
  let signalRService: any;
  let notificationSubject: Subject<string>;

  beforeEach(async () => {
    notificationSubject = new Subject<string>();

    dashboardService = {
      getStats: vi.fn().mockReturnValue(of({ totalUsers: 0, pendingPosts: 0, totalReports: 0, activeGroups: 0, recentActivities: [] }))
    };

    signalRService = {
      startConnection: vi.fn(),
      stopConnection: vi.fn(),
      notification$: notificationSubject.asObservable()
    };

    await TestBed.configureTestingModule({
      declarations: [AdminDashboardComponent],
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        { provide: DashboardService, useValue: dashboardService },
        { provide: SignalRService, useValue: signalRService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load stats on init', () => {
    const mockStats = { totalUsers: 100, pendingPosts: 5, totalReports: 2, activeGroups: 3, recentActivities: [] };
    dashboardService.getStats.mockReturnValue(of(mockStats));

    component.loadStats();

    expect(dashboardService.getStats).toHaveBeenCalled();
  });

  it('should start SignalR connection on init', () => {
    expect(signalRService.startConnection).toHaveBeenCalled();
  });

  it('should stop SignalR connection on destroy', () => {
    component.ngOnDestroy();
    expect(signalRService.stopConnection).toHaveBeenCalled();
  });
});
