export interface ReportSummary {
  monthlyIncome: number;
  monthlyExpenses: number;
  monthlyBalance: number;
  topCategoryName: string | null;
  topCategoryAmount: number;
  insightMessage: string;
}

export interface ReportCategoryBreakdown {
  categoryId: number;
  categoryName: string;
  totalSpent: number;
  percentage: number;
  transactionsCount: number;
}

export interface DailySpending {
  date: string;
  totalSpent: number;
}

export interface IncomeVsExpensesReport {
  month: number;
  income: number;
  expenses: number;
}