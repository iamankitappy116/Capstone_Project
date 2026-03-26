import { Component, OnInit, OnDestroy } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';
import { Router } from '@angular/router';
import { SignalRService } from '../../../core/services/signalr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-admin-dashboard',
  standalone: false,
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboardComponent implements OnInit, OnDestroy {
  stats: any = {
    totalUsers: 0,
    pendingPosts: 0,
    totalReports: 0,
    activeGroups: 0,
    recentActivities: []
  };
  username: string | null = '';
  private notificationSub: Subscription | null = null;

  constructor(
    private dashboardService: DashboardService,
    private router: Router,
    private signalRService: SignalRService
  ) {}

  ngOnInit(): void {
    this.username = sessionStorage.getItem('username');
    this.loadStats();

    // SignalR Setup
    this.signalRService.startConnection();
    this.notificationSub = this.signalRService.notification$.subscribe(message => {
      if (message.includes('post approval request')) {
        alert(message);
        this.loadStats(); // Refresh stats for pending post count
      }
    });
  }

  ngOnDestroy(): void {
    if (this.notificationSub) {
      this.notificationSub.unsubscribe();
    }
    this.signalRService.stopConnection();
  }

  loadStats(): void {
    this.dashboardService.getStats().subscribe({
      next: (data) => {
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
