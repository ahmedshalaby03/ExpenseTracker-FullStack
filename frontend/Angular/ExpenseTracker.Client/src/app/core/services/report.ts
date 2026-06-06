import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  DailySpending,
  IncomeVsExpensesReport,
  ReportCategoryBreakdown,
  ReportSummary
} from '../models/report.models';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private readonly baseUrl = `${environment.apiUrl}/Reports`;

  constructor(private http: HttpClient) {}

  getSummary(month: number, year: number): Observable<ReportSummary> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year);

    return this.http.get<ReportSummary>(`${this.baseUrl}/summary`, { params });
  }

  getCategoryBreakdown(month: number, year: number): Observable<ReportCategoryBreakdown[]> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year);

    return this.http.get<ReportCategoryBreakdown[]>(
      `${this.baseUrl}/category-breakdown`,
      { params }
    );
  }

  getDailySpending(month: number, year: number): Observable<DailySpending[]> {
    const params = new HttpParams()
      .set('month', month)
      .set('year', year);

    return this.http.get<DailySpending[]>(
      `${this.baseUrl}/daily-spending`,
      { params }
    );
  }

  getIncomeVsExpenses(year: number): Observable<IncomeVsExpensesReport[]> {
    const params = new HttpParams().set('year', year);

    return this.http.get<IncomeVsExpensesReport[]>(
      `${this.baseUrl}/income-vs-expenses`,
      { params }
    );
  }

  exportCsv(month: number, year: number) {
  return this.http.get(
    `${this.baseUrl}/export-csv?month=${month}&year=${year}`,
    {
      responseType: 'blob'
    }
  );
}
}