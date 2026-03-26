import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserManagementComponent } from './user-management';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DashboardService } from '../../../core/services/dashboard';
import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';

describe('UserManagementComponent', () => {
  let component: UserManagementComponent;
  let fixture: ComponentFixture<UserManagementComponent>;
  let dashboardService: any;

  beforeEach(async () => {
    dashboardService = {
      getAllUsers: vi.fn().mockReturnValue(of([])),
      createUser: vi.fn().mockReturnValue(of({})),
      updateUser: vi.fn().mockReturnValue(of({})),
      deleteUser: vi.fn().mockReturnValue(of({}))
    };

    await TestBed.configureTestingModule({
      declarations: [UserManagementComponent],
      imports: [HttpClientTestingModule, FormsModule],
      providers: [
        { provide: DashboardService, useValue: dashboardService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on init', () => {
    const mockUsers = [{ userId: 1, username: 'admin' }];
    dashboardService.getAllUsers.mockReturnValue(of(mockUsers));

    component.loadUsers();

    expect(component.users).toEqual(mockUsers);
    expect(dashboardService.getAllUsers).toHaveBeenCalled();
  });

  it('should open create modal', () => {
    component.openCreateModal();
    expect(component.showModal).toBe(true);
    expect(component.isEditMode).toBe(false);
    expect(component.newUser.username).toBe('');
  });

  it('should open edit modal with user data', () => {
    const user = { userId: 1, username: 'testuser', email: 'test@test.com', role: 'User' };
    component.editUser(user);

    expect(component.showModal).toBe(true);
    expect(component.isEditMode).toBe(true);
    expect(component.editingUserId).toBe(1);
    expect(component.newUser.username).toBe('testuser');
  });

  it('should call createUser in saveUser when NOT in edit mode', () => {
    component.isEditMode = false;
    component.newUser = { username: 'new', email: 'new@test.com', password: '123', role: 'User' };

    component.saveUser();

    expect(dashboardService.createUser).toHaveBeenCalledWith(component.newUser);
  });

  it('should call updateUser in saveUser when in edit mode', () => {
    component.isEditMode = true;
    component.editingUserId = 1;
    component.newUser = { username: 'updated', email: 'upd@test.com', role: 'Admin' };

    component.saveUser();

    expect(dashboardService.updateUser).toHaveBeenCalledWith(1, component.newUser);
  });

  it('should delete user if confirmed', () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true);

    component.deleteUser(1);

    expect(dashboardService.deleteUser).toHaveBeenCalledWith(1);
  });
});
