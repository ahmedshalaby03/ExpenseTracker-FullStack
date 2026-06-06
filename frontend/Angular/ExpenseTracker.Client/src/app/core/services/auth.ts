import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';
import { TokenService } from './token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/Auth`;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {}

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/register`, data).pipe(
      tap(response => this.saveAuthData(response))
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data).pipe(
      tap(response => this.saveAuthData(response))
    );
  }

  logout(): void {
    this.tokenService.clear();
  }

  isLoggedIn(): boolean {
    return this.tokenService.isLoggedIn();
  }

  private saveAuthData(response: AuthResponse): void {
    this.tokenService.setToken(response.token);
    this.tokenService.setUser({
      userId: response.userId,
      fullName: response.fullName,
      email: response.email
    });
  }
}