import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private baseUrl = 'http://localhost:5134/api';

  constructor(private http: HttpClient) {}

  private getHeaders() {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getStats(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/dashboard/stats`, { headers: this.getHeaders() });
  }

  getPendingPosts(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/post/pending`, { headers: this.getHeaders() });
  }

  approvePost(id: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/post/${id}/approve`, {}, { headers: this.getHeaders() });
  }

  rejectPost(id: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/post/${id}/reject`, {}, { headers: this.getHeaders() });
  }

  getAllUsers(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/user`, { headers: this.getHeaders() });
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/user/${id}`, { headers: this.getHeaders() });
  }

  getAllReports(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/report`, { headers: this.getHeaders() });
  }

  getGroups(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/group`, { headers: this.getHeaders() });
  }

  createGroup(group: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/group`, group, { headers: this.getHeaders() });
  }

  createUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/user`, user, { headers: this.getHeaders() });
  }
}
