import { Component, OnInit, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { UserService } from '../../core/services/user';
import { Router } from '@angular/router';
import { HostListener } from '@angular/core';
import { SignalRService } from '../../core/services/signalr';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
  encapsulation: ViewEncapsulation.Emulated
})
export class Navbar implements OnInit {
  userProfile: any = null;
  searchQuery: string = '';
  users: any[] = [];
  filteredUsers: any[] = [];
  notificationCount: number = 0;
  notifications: string[] = [];
  showNotifications: boolean = false;

  constructor(
    private signalRService: SignalRService,
    private userService: UserService, 
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(){
    this.loadUserProfile();
    this.signalRService.startConnection();
    this.signalRService.notification$.subscribe(message => {
      this.notifications.unshift(message); // Add to the top
      this.notificationCount++;
      this.cdr.detectChanges();
    });
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

  showDropdown: boolean = false;

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  viewProfile(): void {
    this.router.navigate(['/dashboard/profile']);
    this.showDropdown = false;
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/login']);
    this.showDropdown = false;
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const target = event.target as HTMLElement;

    if (!target.closest('.profile-container')) {
      this.showDropdown = false;
    }

    if (!target.closest('.notification-icon')) {
      this.showNotifications = false;
    }
  }

  toggleNotifications(): void {
    this.showNotifications = !this.showNotifications;
    if (this.showNotifications) {
      this.notificationCount = 0; // Reset count when viewed
    }
  }

  clearNotifications(): void {
    this.notifications = [];
    this.showNotifications = false;
  }
}
