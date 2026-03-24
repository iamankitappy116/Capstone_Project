import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { GroupService } from '../../core/services/group';
import { Router } from '@angular/router';

@Component({
  selector: 'app-groups',
  standalone: false,
  templateUrl: './groups.html',
  styleUrl: './groups.css'
})
export class Groups implements OnInit {
  myGroups: any[] = [];
  suggestedGroups: any[] = [];
  searchQuery: string = '';
  currentUserId: number = 0;

  constructor(
    private groupService: GroupService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }

  ngOnInit(): void {
    const userIdStr = sessionStorage.getItem('userId');
    if (userIdStr) {
      this.currentUserId = parseInt(userIdStr);
      this.loadGroups();
    }
  }

  loadGroups(): void {
    // Load all groups and user's joined groups
    this.groupService.getUserGroups(this.currentUserId).subscribe(userGroups => {
      this.myGroups = userGroups;

      this.groupService.getGroups().subscribe(allGroups => {
        // Suggested groups are groups the user hasn't joined yet
        const joinedIds = new Set(this.myGroups.map(g => g.groupId));
        this.suggestedGroups = allGroups.filter(g => !joinedIds.has(g.groupId));
        this.cdr.detectChanges();
      });
    });
  }

  joinGroup(groupId: number): void {
    this.groupService.joinGroup(groupId, this.currentUserId).subscribe({
      next: () => {
        this.loadGroups();
      },
      error: (err) => console.error('Error joining group:', err)
    });
  }

  viewGroup(groupId: number): void {
    // Navigate to shared group posts/details if implemented
    // For now, staying on the same page or navigating to feed with filter
    console.log('Viewing group:', groupId);
  }

  getGroupInitial(name: string): string {
    return name ? name.charAt(0).toUpperCase() : 'G';
  }

  getFilteredSuggestedGroups() {
    if (!this.searchQuery.trim()) return this.suggestedGroups;
    return this.suggestedGroups.filter(g =>
      g.groupName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      (g.category && g.category.toLowerCase().includes(this.searchQuery.toLowerCase()))
    );
  }
}
