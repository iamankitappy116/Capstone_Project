import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: false,
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboardComponent implements OnInit {
  stats: any = {
    totalUsers: 0,
    pendingPosts: 0,
    totalReports: 0,
    activeGroups: 0,
    recentActivities: []
  };
  username: string | null = '';

  constructor(
    private dashboardService: DashboardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.username = sessionStorage.getItem('username');
    this.loadStats();
  }

  loadStats(): void {
    this.dashboardService.getStats().subscribe({
      next: (data) => {
        console.log('Keys in data:', Object.keys(data));
        console.log('Full data object:', JSON.stringify(data));

        this.stats = {
          totalUsers: data.totalUsers ?? data.TotalUsers ?? 0,
          pendingPosts: data.pendingPosts ?? data.PendingPosts ?? 0,
          totalReports: data.totalReports ?? data.TotalReports ?? 0,
          activeGroups: data.activeGroups ?? data.ActiveGroups ?? 0,
          recentActivities: (data.recentActivities ?? data.RecentActivities ?? []).map((a: any) => ({
            userName: a.userName ?? a.UserName ?? '',
            action: a.action ?? a.Action ?? '',
            timeAgo: a.timeAgo ?? a.TimeAgo ?? ''
          }))
        };
        console.log('Mapped stats:', this.stats);
      },
      error: (err) => {
        console.error('Error loading dashboard stats', err);
      }
    });
  }

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
