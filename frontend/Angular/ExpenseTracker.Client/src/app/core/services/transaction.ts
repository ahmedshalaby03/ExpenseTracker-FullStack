import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateTransactionRequest,
  PagedResult,
  Transaction,
  TransactionFilter,
  UpdateTransactionRequest
} from '../models/transaction.models';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly baseUrl = `${environment.apiUrl}/Transactions`;

  constructor(private http: HttpClient) {}

  getAll(filter: TransactionFilter): Observable<PagedResult<Transaction>> {
    let params = new HttpParams();

    if (filter.search) {
      params = params.set('search', filter.search);
    }

    if (filter.fromDate) {
      params = params.set('fromDate', filter.fromDate);
    }

    if (filter.toDate) {
      params = params.set('toDate', filter.toDate);
    }

    if (filter.type) {
      params = params.set('type', filter.type);
    }

    if (filter.categoryId) {
      params = params.set('categoryId', filter.categoryId);
    }

    if (filter.paymentMethod) {
      params = params.set('paymentMethod', filter.paymentMethod);
    }

    params = params
      .set('pageNumber', filter.pageNumber ?? 1)
      .set('pageSize', filter.pageSize ?? 10);

    return this.http.get<PagedResult<Transaction>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateTransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.baseUrl, request);
  }

  update(id: number, request: UpdateTransactionRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}