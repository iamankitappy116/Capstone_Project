import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard';

@Component({
  selector: 'app-reported-users',
  standalone: false,
  templateUrl: './reported-users.html',
  styleUrl: './reported-users.css'
})
export class ReportedUsersComponent implements OnInit {
  reports: any[] = [];
  loading: boolean = false;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    this.dashboardService.getAllReports().subscribe({
      next: (data) => {
        this.reports = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading reports', err);
        this.loading = false;
      }
    });
  }

  warnUser(userId: number): void {
    alert('Warning sent to user #' + userId);
  }

  deleteUser(userId: number): void {
    if (confirm('Are you sure you want to delete this reported user?')) {
      this.dashboardService.deleteUser(userId).subscribe({
        next: () => this.loadReports(),
        error: (err) => alert('Error: ' + err.message)
      });
    }
  }
}
