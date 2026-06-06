import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  Category,
  CategorySummary,
  CreateCategoryRequest,
  UpdateCategoryRequest
} from '../models/category.models';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly baseUrl = `${environment.apiUrl}/Categories`;

  constructor(private http: HttpClient) {}

  getAll(search?: string, type?: number | null): Observable<Category[]> {
    let params = new HttpParams();

    if (search) {
      params = params.set('search', search);
    }

    if (type) {
      params = params.set('type', type);
    }

    return this.http.get<Category[]>(this.baseUrl, { params });
  }

  getSummary(): Observable<CategorySummary> {
    return this.http.get<CategorySummary>(`${this.baseUrl}/summary`);
  }

  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateCategoryRequest): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, request);
  }

  update(id: number, request: UpdateCategoryRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}