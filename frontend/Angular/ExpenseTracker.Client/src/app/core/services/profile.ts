import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ChangePasswordRequest,
  Profile,
  UpdatePreferencesRequest,
  UpdateProfileRequest
} from '../models/profile.models';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private readonly baseUrl = `${environment.apiUrl}/Profile`;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<Profile> {
    return this.http.get<Profile>(this.baseUrl);
  }

  updateProfile(request: UpdateProfileRequest): Observable<Profile> {
    return this.http.put<Profile>(this.baseUrl, request);
  }

  uploadAvatar(file: File) {
  const formData = new FormData();
  formData.append('file', file);

  return this.http.post<{ avatarUrl: string; fullAvatarUrl: string }>(
    `${this.baseUrl}/avatar`,
    formData
  );
}
  updatePreferences(request: UpdatePreferencesRequest): Observable<Profile> {
    return this.http.put<Profile>(`${this.baseUrl}/preferences`, request);
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/change-password`, request);
  }
}