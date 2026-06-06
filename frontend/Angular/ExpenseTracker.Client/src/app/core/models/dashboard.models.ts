export interface DashboardSummary {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  savingRate: number;
  totalTransactions: number;
}

export interface RecentTransaction {
  id: number;
  amount: number;
  type: number;
  description: string | null;
  transactionDate: string;
  paymentMethod: number;
  categoryName: string;
}

export interface ExpenseByCategory {
  categoryId: number;
  categoryName: string;
  totalAmount: number;
}

export interface MonthlyIncomeExpense {
  year: number;
  month: number;
  totalIncome: number;
  totalExpenses: number;
}

export interface TopCategory {
  categoryId: number;
  categoryName: string;
  totalAmount: number;
}