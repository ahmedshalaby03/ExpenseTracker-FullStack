import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { combineLatest, of } from 'rxjs';
import { catchError, filter, take } from 'rxjs/operators';

import { ReportService } from '../../core/services/report';
import {
  DailySpending,
  IncomeVsExpensesReport,
  ReportCategoryBreakdown,
  ReportSummary
} from '../../core/models/report.models';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe, DatePipe],
  templateUrl: './reports.html',
  styleUrl: './reports.css'
})
export class ReportsComponent implements OnInit {
  summary: ReportSummary | null = null;
  categoryBreakdown: ReportCategoryBreakdown[] = [];
  dailySpending: DailySpending[] = [];
  incomeVsExpenses: IncomeVsExpensesReport[] = [];

  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();

  isLoading = false;
  errorMessage = '';

  months = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  years = [2023, 2024, 2025, 2026];

  constructor(
    private reportService: ReportService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.isLoading = true;
    this.errorMessage = '';

    combineLatest({
      summary: this.reportService.getSummary(this.selectedMonth, this.selectedYear).pipe(
        take(1),
        catchError(() => of(null))
      ),
      categoryBreakdown: this.reportService.getCategoryBreakdown(this.selectedMonth, this.selectedYear).pipe(
        take(1),
        catchError(() => of([]))
      ),
      dailySpending: this.reportService.getDailySpending(this.selectedMonth, this.selectedYear).pipe(
        take(1),
        catchError(() => of([]))
      ),
      incomeVsExpenses: this.reportService.getIncomeVsExpenses(this.selectedYear).pipe(
        take(1),
        catchError(() => of([]))
      )
    }).pipe(
      filter(data =>
        data.summary !== undefined &&
        data.categoryBreakdown !== undefined &&
        data.dailySpending !== undefined &&
        data.incomeVsExpenses !== undefined
      ),
      take(1)
    ).subscribe({
      next: (data) => {
        this.summary = data.summary;
        this.categoryBreakdown = data.categoryBreakdown ?? [];
        this.dailySpending = data.dailySpending ?? [];
        this.incomeVsExpenses = data.incomeVsExpenses ?? [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load reports';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onFilterChange(): void {
    this.loadReports();
  }

  getMonthName(month: number): string {
    return this.months.find(m => m.value === month)?.name.slice(0, 3) ?? '';
  }

  getMaxIncomeExpenseValue(): number {
    const values = this.incomeVsExpenses.flatMap(item => [item.income, item.expenses]);
    return Math.max(...values, 1);
  }

  getIncomeBarHeight(value: number): number {
    return (value / this.getMaxIncomeExpenseValue()) * 100;
  }

  getExpenseBarHeight(value: number): number {
    return (value / this.getMaxIncomeExpenseValue()) * 100;
  }

  getMaxDailySpending(): number {
    const values = this.dailySpending.map(item => item.totalSpent);
    return Math.max(...values, 1);
  }

  getDailyBarHeight(value: number): number {
    return (value / this.getMaxDailySpending()) * 100;
  }

  getTotalCategorySpent(): number {
    return this.categoryBreakdown.reduce((sum, item) => sum + item.totalSpent, 0);
  }

 exportPdf(): void {
  window.print();
}

  exportExcel(): void {
  this.reportService.exportCsv(this.selectedMonth, this.selectedYear).subscribe({
    next: (blob) => {
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = `expense-report-${this.selectedYear}-${this.selectedMonth}.csv`;
      a.click();

      window.URL.revokeObjectURL(url);
    },
    error: () => {
      this.errorMessage = 'Failed to export report';
    }
  });
}
}