export interface Category {
  id: number;
  name: string;
  type: number;
  icon?: string | null;
  color?: string | null;
  transactionsCount: number;
  totalAmount: number;
}

export interface CreateCategoryRequest {
  name: string;
  type: number;
  icon?: string | null;
  color?: string | null;
}

export interface UpdateCategoryRequest {
  name: string;
  type: number;
  icon?: string | null;
  color?: string | null;
}

export interface CategorySummary {
  expenseCategoriesCount: number;
  incomeCategoriesCount: number;
  usedCategoriesCount: number;
  totalCategoriesCount: number;
  utilizationPercentage: number;
}