import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReportedUsersComponent } from './reported-users';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DashboardService } from '../../../core/services/dashboard';
import { of } from 'rxjs';

describe('ReportedUsersComponent', () => {
  let component: ReportedUsersComponent;
  let fixture: ComponentFixture<ReportedUsersComponent>;
  let dashboardService: any;

  beforeEach(async () => {
    dashboardService = {
      getAllReports: vi.fn().mockReturnValue(of([])),
      deleteUser: vi.fn().mockReturnValue(of({}))
    };

    await TestBed.configureTestingModule({
      declarations: [ReportedUsersComponent],
      imports: [HttpClientTestingModule],
      providers: [
        { provide: DashboardService, useValue: dashboardService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ReportedUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load reports on init', () => {
    const mockReports = [{ reportedUserId: 1, reportedUserName: 'baduser', reason: 'Spam' }];
    dashboardService.getAllReports.mockReturnValue(of(mockReports));

    component.loadReports();

    expect(component.reports).toEqual(mockReports);
    expect(dashboardService.getAllReports).toHaveBeenCalled();
  });

  it('should delete user if confirmed', () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true);

    component.deleteUser(1);

    expect(dashboardService.deleteUser).toHaveBeenCalledWith(1);
  });
});
