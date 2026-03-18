import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private apiUrl = 'http://localhost:5134/api/group';

  constructor(private http: HttpClient) { }

  getGroups(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getUserGroups(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user/${userId}`);
  }

  joinGroup(groupId: number, userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/join`, { groupId, userId }, { responseType: 'text' });
  }

  createGroup(groupData: any): Observable<any> {
    return this.http.post(this.apiUrl, groupData);
  }
}
