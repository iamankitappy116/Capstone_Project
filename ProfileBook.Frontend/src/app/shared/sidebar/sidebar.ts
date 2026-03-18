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
    const role = localStorage.getItem('role');
    this.isAdmin = role === 'Admin';
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }
}
