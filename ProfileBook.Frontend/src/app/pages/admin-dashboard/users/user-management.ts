import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';

@Component({
  selector: 'app-user-management',
  standalone: false,
  templateUrl: './user-management.html',
  styleUrl: './user-management.css'
})
export class UserManagementComponent implements OnInit {
  users: any[] = [];
  loading: boolean = false;
  showModal: boolean = false;
  newUser: any = {
    username: '',
    email: '',
    password: '',
    role: 'User'
  };
  isEditMode: boolean = false;
  editingUserId: number | null = null;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.dashboardService.getAllUsers().subscribe({
      next: (data) => {
        console.log('User list response:', data);
        if (data && data.length > 0) {
          console.log('First user keys:', Object.keys(data[0]));
        }
        this.users = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading users', err);
        this.loading = false;
      }
    });
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.dashboardService.deleteUser(id).subscribe({
        next: () => {
          this.loadUsers();
        },
        error: (err) => {
          alert('Error deleting user: ' + err.message);
        }
      });
    }
  }

  editUser(user: any): void {
    this.isEditMode = true;
    this.editingUserId = user.userId;
    this.newUser = { 
      username: user.username, 
      email: user.email, 
      password: '', // Password not required for edit
      role: user.role 
    };
    this.showModal = true;
  }

  openCreateModal(): void {
    this.isEditMode = false;
    this.editingUserId = null;
    this.newUser = { username: '', email: '', password: '', role: 'User' };
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  saveUser(): void {
    if (!this.newUser.username || !this.newUser.email || (!this.isEditMode && !this.newUser.password)) {
      alert('Please fill in all required fields.');
      return;
    }

    if (this.isEditMode && this.editingUserId) {
      this.dashboardService.updateUser(this.editingUserId, this.newUser).subscribe({
        next: () => {
          alert('User updated successfully!');
          this.closeModal();
          this.loadUsers();
        },
        error: (err) => {
          alert('Error updating user: ' + (err.error || err.message));
        }
      });
    } else {
      this.dashboardService.createUser(this.newUser).subscribe({
        next: () => {
          alert('User created successfully!');
          this.closeModal();
          this.loadUsers();
        },
        error: (err) => {
          alert('Error creating user: ' + (err.error || err.message));
        }
      });
    }
  }
}
