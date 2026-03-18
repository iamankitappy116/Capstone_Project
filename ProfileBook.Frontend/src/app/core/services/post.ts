import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private apiUrl = 'http://localhost:5134/api/post';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getPosts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  createPost(content: string, mediaUrl?: string, mediaType?: string): Observable<any> {
    const userId = Number(localStorage.getItem('userId')) || 0;
    const payload = {
      content: content,
      userId: userId,
      mediaUrl: mediaUrl,
      mediaType: mediaType
    };
    return this.http.post<any>(this.apiUrl, payload, { headers: this.getHeaders() });
  }

  uploadMedia(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(`${this.apiUrl}/upload`, formData, { headers: this.getHeaders() });
  }

  getPostsByUser(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user/${userId}`, { headers: this.getHeaders() });
  }

  likePost(postId: number): Observable<any> {
    const userId = Number(localStorage.getItem('userId')) || 0;
    return this.http.post<any>(`http://localhost:5134/api/like`, { userId, postId }, { headers: this.getHeaders() });
  }

  removeLike(postId: number): Observable<any> {
    const userId = Number(localStorage.getItem('userId')) || 0;
    return this.http.delete<any>(`http://localhost:5134/api/like?userId=${userId}&postId=${postId}`, { headers: this.getHeaders() });
  }

  getComments(postId: number): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5134/api/comment/post/${postId}`, { headers: this.getHeaders() });
  }

  addComment(postId: number, commentContent: string): Observable<any> {
    const userId = Number(localStorage.getItem('userId')) || 0;
    return this.http.post<any>(`http://localhost:5134/api/comment`, { userId, postId, commentText: commentContent }, { headers: this.getHeaders() });
  }
}
