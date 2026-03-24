import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar implements OnInit {
  isAdmin: boolean = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const role = sessionStorage.getItem('role');
    this.isAdmin = role === 'Admin';
  }

  logout() {
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('role');
    this.router.navigate(['/login']);
  }
}
