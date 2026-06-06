import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DashboardService } from '../../core/services/dashboard';
import {
  DashboardSummary,
  ExpenseByCategory,
  MonthlyIncomeExpense,
  RecentTransaction,
  TopCategory
} from '../../core/models/dashboard.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  summary: DashboardSummary | null = null;
  recentTransactions: RecentTransaction[] = [];
  expensesByCategory: ExpenseByCategory[] = [];
  monthlyData: MonthlyIncomeExpense[] = [];
  topCategory: TopCategory | null = null;

  isLoading = false;
  errorMessage = '';

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.errorMessage = '';
    const currentYear = new Date().getFullYear();

    forkJoin({
      summary: this.dashboardService.getSummary(),
      transactions: this.dashboardService.getRecentTransactions(5),
      categories: this.dashboardService.getExpensesByCategory(),
      monthly: this.dashboardService.getMonthlyIncomeExpense(currentYear),
      topCategory: this.dashboardService.getTopCategory()
    }).subscribe({
      next: (data) => {
        this.summary = data.summary;
        this.recentTransactions = data.transactions;
        this.expensesByCategory = data.categories;
        this.monthlyData = data.monthly;
        this.topCategory = data.topCategory;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load dashboard';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getTransactionType(type: number): string {
    return type === 1 ? 'Income' : 'Expense';
  }

  getPaymentMethod(method: number): string {
    switch (method) {
      case 1: return 'Cash';
      case 2: return 'Card';
      case 3: return 'Wallet';
      case 4: return 'Bank Transfer';
      default: return 'Unknown';
    }
  }

  getTotalCategoryExpenses(): number {
    return this.expensesByCategory.reduce((sum, item) => sum + item.totalAmount, 0);
  }

  getCategoryPercentage(amount: number): number {
    const total = this.getTotalCategoryExpenses();
    if (total === 0) return 0;
    return Math.round((amount / total) * 100);
  }

  getMaxMonthlyValue(): number {
    const values = this.monthlyData.flatMap(item => [
      item.totalIncome,
      item.totalExpenses
    ]);
    return Math.max(...values, 1);
  }

  getIncomeBarHeight(value: number): number {
    return (value / this.getMaxMonthlyValue()) * 100;
  }

  getExpenseBarHeight(value: number): number {
    return (value / this.getMaxMonthlyValue()) * 100;
  }

  getMonthName(month: number): string {
    const months = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
    return months[month - 1] ?? '';
  }
}