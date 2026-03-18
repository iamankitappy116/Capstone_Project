import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';

@Component({
  selector: 'app-post-approvals',
  standalone: false,
  templateUrl: './post-approvals.html',
  styleUrl: './post-approvals.css'
})
export class PostApprovalsComponent implements OnInit {
  pendingPosts: any[] = [];
  loading: boolean = false;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadPendingPosts();
  }

  loadPendingPosts(): void {
    this.loading = true;
    this.dashboardService.getPendingPosts().subscribe({
      next: (data) => {
        console.log('Pending posts response:', data);
        if (data && data.length > 0) {
          console.log('First post keys:', Object.keys(data[0]));
        }
        this.pendingPosts = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading pending posts', err);
        this.loading = false;
      }
    });
  }

  approve(id: number): void {
    this.dashboardService.approvePost(id).subscribe({
      next: () => this.loadPendingPosts(),
      error: (err) => alert('Error: ' + err.message)
    });
  }

  reject(id: number): void {
    this.dashboardService.rejectPost(id).subscribe({
      next: () => this.loadPendingPosts(),
      error: (err) => alert('Error: ' + err.message)
    });
  }
}
