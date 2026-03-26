import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private apiUrl = 'http://localhost:5134/api/group';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = sessionStorage.getItem('token');
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  getGroups(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  getUserGroups(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user/${userId}`, { headers: this.getHeaders() });
  }

  joinGroup(groupId: number, userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/join`, { groupId, userId }, { headers: this.getHeaders(), responseType: 'text' });
  }

  createGroup(groupData: any): Observable<any> {
    return this.http.post(this.apiUrl, groupData, { headers: this.getHeaders() });
  }
}
