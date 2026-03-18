import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../core/services/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit {
  userProfile: any = null;
  searchQuery: string = '';
  users: any[] = [];
  filteredUsers: any[] = [];

  constructor(
    private userService: UserService, 
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.userService.getMyProfile().subscribe({
      next: (data) => {
        this.userProfile = data;
        this.cdr.detectChanges();
      },
    });
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe({
      next: (data: any[]) => {
        this.users = data;
      },
      error: (err: any) => console.error('Error loading users for search:', err)
    });
  }

  onSearchChange(): void {
    if (!this.searchQuery.trim()) {
      this.filteredUsers = [];
      return;
    }
    const query = this.searchQuery.toLowerCase();
    this.filteredUsers = this.users.filter(u => 
      u.username.toLowerCase().includes(query) && u.userId !== (this.userProfile?.userId || 0)
    ).slice(0, 5); // Limit to top 5 results
  }

  startChat(user: any): void {
    this.searchQuery = '';
    this.filteredUsers = [];
    this.router.navigate(['/dashboard/messages'], { queryParams: { userId: user.userId } });
  }

  getProfileImage(): string {
    if (this.userProfile?.profileImage) {
      return `http://localhost:5134${this.userProfile.profileImage}`;
    }
    const name = this.userProfile?.username || 'User';
    return `https://ui-avatars.com/api/?name=${name}&background=random`;
  }
}
