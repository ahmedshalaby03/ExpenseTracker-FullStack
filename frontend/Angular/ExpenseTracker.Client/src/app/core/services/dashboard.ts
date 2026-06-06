import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  DashboardSummary,
  ExpenseByCategory,
  MonthlyIncomeExpense,
  RecentTransaction,
  TopCategory
} from '../models/dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly baseUrl = `${environment.apiUrl}/Dashboard`;

  constructor(private http: HttpClient) {}

  getSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${this.baseUrl}/summary`);
  }

  getRecentTransactions(count: number = 5): Observable<RecentTransaction[]> {
    return this.http.get<RecentTransaction[]>(
      `${this.baseUrl}/recent-transactions?count=${count}`
    );
  }

  getExpensesByCategory(): Observable<ExpenseByCategory[]> {
    return this.http.get<ExpenseByCategory[]>(
      `${this.baseUrl}/expenses-by-category`
    );
  }

  getMonthlyIncomeExpense(year: number): Observable<MonthlyIncomeExpense[]> {
    return this.http.get<MonthlyIncomeExpense[]>(
      `${this.baseUrl}/monthly-income-expense?year=${year}`
    );
  }

  getTopCategory(): Observable<TopCategory | null> {
    return this.http.get<TopCategory | null>(`${this.baseUrl}/top-category`);
  }
}