import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FollowService {
  private apiUrl = 'http://localhost:5134/api/follow';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  follow(followingId: number): Observable<any> {
    const followerId = Number(sessionStorage.getItem('userId')) || 0;
    return this.http.post<any>(this.apiUrl, { followerId, followingId }, { headers: this.getHeaders() });
  }

  unfollow(followingId: number): Observable<any> {
    const followerId = Number(sessionStorage.getItem('userId')) || 0;
    return this.http.delete<any>(`${this.apiUrl}?followerId=${followerId}&followingId=${followingId}`, { headers: this.getHeaders() });
  }

  isFollowing(followingId: number): Observable<{ isFollowing: boolean }> {
    const followerId = Number(sessionStorage.getItem('userId')) || 0;
    return this.http.get<{ isFollowing: boolean }>(`${this.apiUrl}?followerId=${followerId}&followingId=${followingId}`, { headers: this.getHeaders() });
  }
}
