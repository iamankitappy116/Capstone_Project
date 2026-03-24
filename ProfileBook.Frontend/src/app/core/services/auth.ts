import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = 'http://localhost:5134/api/auth';
  constructor(private http: HttpClient) {}
    login(loginData: any): Observable<any> {
      return this.http.post<any>(`${this.apiUrl}/login`, loginData).pipe(
        tap((response: any) => {
          if(response && response.token){
            sessionStorage.setItem('token', response.token);
            sessionStorage.setItem('username', response.username);
            sessionStorage.setItem('userId', response.userId);
            sessionStorage.setItem('role', response.role);
          }  
        })
      );
    }
    register(registerData: any): Observable<any> {
      const payload = {
        username: registerData.username,
        password: registerData.password,
        email: registerData.email,
      };
      return this.http.post<any>(`${this.apiUrl}/register`, payload);
    }
    isLoggedIn(): boolean {
      return !!sessionStorage.getItem('token');
    }
}
