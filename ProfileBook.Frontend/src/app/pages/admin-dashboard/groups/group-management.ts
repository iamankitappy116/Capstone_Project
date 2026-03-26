import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';

@Component({
  selector: 'app-group-management',
  standalone: false,
  templateUrl: './group-management.html',
  styleUrl: './group-management.css'
})
export class GroupManagementComponent implements OnInit {
  groups: any[] = [];
  stats: any = {
    totalGroups: 0,
    totalMembers: 0,
    totalPosts: 0
  };
  searchQuery: string = '';
  showCreateModal: boolean = false;
  users: any[] = [];
  searchUser: string = '';
  newGroup: any = {
    groupName: '',
    description: '',
    category: '',
    createdByUserId: 0,
    memberIds: []
  };

  showAddMemberModal: boolean = false;
  selectedGroupForMember: any = null;
  searchUserForGroup: string = '';
  
  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    const userId = sessionStorage.getItem('userId'); // Fixed: was localStorage
    this.newGroup.createdByUserId = userId ? parseInt(userId) : 0;
    this.loadGroups();
    this.loadUsers();
  }

  loadUsers(): void {
    this.dashboardService.getAllUsers().subscribe({
      next: (data: any[]) => this.users = data,
      error: (err: any) => console.error(err)
    });
  }

  toggleMemberSelection(userId: number): void {
    const index = this.newGroup.memberIds.indexOf(userId);
    if (index > -1) {
      this.newGroup.memberIds.splice(index, 1);
    } else {
      this.newGroup.memberIds.push(userId);
    }
  }

  get filteredUsers() {
    return this.users.filter(u => 
      u.username.toLowerCase().includes(this.searchUser.toLowerCase()) && 
      u.userId !== this.newGroup.createdByUserId
    );
  }

  loadGroups(): void {
    this.dashboardService.getGroups().subscribe({
      next: (data) => {
        this.groups = data;
        this.calculateStats();
      },
      error: (err) => console.error(err)
    });
  }

  calculateStats(): void {
    this.stats.totalGroups = this.groups.length;
    this.stats.totalMembers = this.groups.reduce((acc, g: any) => acc + g.memberCount, 0);
    this.stats.totalPosts = this.groups.reduce((acc, g: any) => acc + g.postCount, 0);
  }

  toggleCreateModal(): void {
    this.showCreateModal = !this.showCreateModal;
  }

  onSubmit(): void {
    if (!this.newGroup.groupName) return;

    this.dashboardService.createGroup(this.newGroup).subscribe({
      next: () => {
        this.loadGroups();
        this.showCreateModal = false;
        this.newGroup.groupName = '';
        this.newGroup.description = '';
        this.newGroup.category = '';
        this.newGroup.memberIds = [];
      },
      error: (err) => alert('Error creating group: ' + err.message)
    });
  }

  get filteredGroups() {
    return this.groups.filter(g => 
      g.groupName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      g.category.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  openAddMemberModal(group: any): void {
    this.selectedGroupForMember = group;
    this.showAddMemberModal = true;
    this.searchUserForGroup = '';
  }

  closeAddMemberModal(): void {
    this.showAddMemberModal = false;
    this.selectedGroupForMember = null;
  }

  get filteredUsersForGroup() {
    return this.users.filter(u =>
      u.username.toLowerCase().includes(this.searchUserForGroup.toLowerCase())
    );
  }

  addUserToExistingGroup(userId: number): void {
    if (!this.selectedGroupForMember) return;
    const payload = {
      groupId: this.selectedGroupForMember.groupId,
      userId: userId
    };
    this.dashboardService.addUserToGroup(payload).subscribe({
      next: () => {
        alert('User added successfully!');
        this.loadGroups();
        this.closeAddMemberModal();
      },
      error: (err: any) => alert('Error adding user: ' + (err.error || err.message))
    });
  }
}
