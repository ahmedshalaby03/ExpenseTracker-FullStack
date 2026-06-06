export interface Transaction {
  id: number;
  amount: number;
  type: number;
  description: string | null;
  transactionDate: string;
  paymentMethod: number;
  categoryId: number;
  categoryName: string;
}

export interface CreateTransactionRequest {
  amount: number;
  type: number;
  description?: string | null;
  transactionDate: string;
  paymentMethod: number;
  categoryId: number;
}

export interface UpdateTransactionRequest {
  amount: number;
  type: number;
  description?: string | null;
  transactionDate: string;
  paymentMethod: number;
  categoryId: number;
}

export interface TransactionFilter {
  search?: string;
  fromDate?: string;
  toDate?: string;
  type?: number | null;
  categoryId?: number | null;
  paymentMethod?: number | null;
  pageNumber?: number;
  pageSize?: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}