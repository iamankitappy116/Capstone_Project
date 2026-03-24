import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayoutComponent implements OnInit {
  username: string | null = '';

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.username = sessionStorage.getItem('username');
  }

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
