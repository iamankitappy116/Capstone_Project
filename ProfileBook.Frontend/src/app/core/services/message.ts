import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = 'http://localhost:5134/api/message';

  constructor(private http: HttpClient) { }

  getInbox(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/inbox/${userId}`);
  }

  getConversation(user1Id: number, user2Id: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/conversation?user1Id=${user1Id}&user2Id=${user2Id}`);
  }

  sendMessage(messageData: { senderId: number, receiverId: number, messageContent: string }): Observable<any> {
    return this.http.post<any>(this.apiUrl, messageData);
  }
}
