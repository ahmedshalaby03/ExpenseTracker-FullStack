import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { forkJoin, of, combineLatest } from 'rxjs';
import { catchError, filter, take } from 'rxjs/operators';

import { TransactionService } from '../../core/services/transaction';
import { CategoryService } from '../../core/services/category';
import { Category } from '../../core/models/category.models';
import { Transaction, TransactionFilter } from '../../core/models/transaction.models';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, CurrencyPipe, DatePipe],
  templateUrl: './transactions.html',
  styleUrl: './transactions.css'
})
export class TransactionsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private transactionService = inject(TransactionService);
  private categoryService = inject(CategoryService);
  private cdr = inject(ChangeDetectorRef);

  transactions: Transaction[] = [];
  categories: Category[] = [];

  isLoading = false;
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  selectedTransactionId: number | null = null;
  showForm = false;

  totalCount = 0;
  pageNumber = 1;
  pageSize = 5;

  filter: TransactionFilter = {
    search: '',
    fromDate: '',
    toDate: '',
    type: null,
    categoryId: null,
    paymentMethod: null,
    pageNumber: 1,
    pageSize: 5
  };

  transactionForm = this.fb.group({
    amount: [0, [Validators.required, Validators.min(1)]],
    type: [2, [Validators.required]],
    description: [''],
    transactionDate: ['', [Validators.required]],
    paymentMethod: [1, [Validators.required]],
    categoryId: [null as number | null, [Validators.required]]
  });

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.filter.pageNumber = this.pageNumber;
    this.filter.pageSize = this.pageSize;

    combineLatest({
      categories: this.categoryService.getAll().pipe(
        take(1),
        catchError(() => of([] as Category[]))
      ),
      transactionsResult: this.transactionService.getAll(this.filter).pipe(
        take(1),
        catchError(() => of({ items: [], totalCount: 0 }))
      )
    }).pipe(
      filter(data => data.categories !== null && data.transactionsResult !== null),
      take(1)
    ).subscribe({
      next: (data) => {
        this.categories = data.categories ?? [];
        this.transactions = data.transactionsResult.items;
        this.totalCount = data.transactionsResult.totalCount;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load transactions';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadTransactions(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.filter.pageNumber = this.pageNumber;
    this.filter.pageSize = this.pageSize;

    this.transactionService.getAll(this.filter).pipe(
      take(1)
    ).subscribe({
      next: (result) => {
        this.transactions = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load transactions';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getAll().pipe(take(1)).subscribe({
      next: (data) => {
        this.categories = data ?? [];
        this.cdr.detectChanges();
      },
      error: () => {
        this.categories = [];
      }
    });
  }

  openAddForm(): void {
    this.showForm = true;
    this.selectedTransactionId = null;

    this.transactionForm.reset({
      amount: 0,
      type: 2,
      description: '',
      transactionDate: new Date().toISOString().substring(0, 10),
      paymentMethod: 1,
      categoryId: null
    });
  }

  edit(transaction: Transaction): void {
    this.showForm = true;
    this.selectedTransactionId = transaction.id;

    this.transactionForm.patchValue({
      amount: transaction.amount,
      type: transaction.type,
      description: transaction.description,
      transactionDate: transaction.transactionDate.substring(0, 10),
      paymentMethod: transaction.paymentMethod,
      categoryId: transaction.categoryId
    });
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.transactionForm.invalid) {
      this.transactionForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    const request = {
      amount: Number(this.transactionForm.value.amount),
      type: Number(this.transactionForm.value.type),
      description: this.transactionForm.value.description || null,
      transactionDate: this.transactionForm.value.transactionDate!,
      paymentMethod: Number(this.transactionForm.value.paymentMethod),
      categoryId: Number(this.transactionForm.value.categoryId)
    };

    if (this.selectedTransactionId) {
      this.transactionService.update(this.selectedTransactionId, request).subscribe({
        next: () => {
          this.isSaving = false;
          this.successMessage = 'Transaction updated successfully';
          this.closeForm();
          this.loadTransactions();
        },
        error: (error) => this.handleError(error)
      });

      return;
    }

    this.transactionService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.successMessage = 'Transaction created successfully';
        this.closeForm();
        this.loadTransactions();
      },
      error: (error) => this.handleError(error)
    });
  }

  delete(transaction: Transaction): void {
    const confirmed = confirm(`Delete transaction "${transaction.description}"?`);

    if (!confirmed) return;

    this.transactionService.delete(transaction.id).subscribe({
      next: () => {
        this.successMessage = 'Transaction deleted successfully';
        this.loadTransactions();
      },
      error: (error) => this.handleError(error)
    });
  }

  applyFilter(): void {
    this.pageNumber = 1;
    this.loadTransactions();
  }

  resetFilter(): void {
    this.filter = {
      search: '',
      fromDate: '',
      toDate: '',
      type: null,
      categoryId: null,
      paymentMethod: null,
      pageNumber: 1,
      pageSize: this.pageSize
    };

    this.pageNumber = 1;
    this.loadTransactions();
  }

  nextPage(): void {
    if (this.pageNumber * this.pageSize >= this.totalCount) return;
    this.pageNumber++;
    this.loadTransactions();
  }

  previousPage(): void {
    if (this.pageNumber === 1) return;
    this.pageNumber--;
    this.loadTransactions();
  }

  closeForm(): void {
    this.showForm = false;
    this.selectedTransactionId = null;
    this.isSaving = false;
  }

  getTypeName(type: number): string {
    return type === 1 ? 'Income' : 'Expense';
  }

  getPaymentMethodName(method: number): string {
    switch (method) {
      case 1: return 'Cash';
      case 2: return 'Card';
      case 3: return 'Wallet';
      case 4: return 'Bank Transfer';
      default: return 'Unknown';
    }
  }

  getFilteredCategories(): Category[] {
    const selectedType = Number(this.transactionForm.value.type);
    return this.categories.filter((c) => c.type === selectedType);
  }

  private handleError(error: any): void {
    this.isSaving = false;

    if (Array.isArray(error.error)) {
      this.errorMessage = error.error.join(', ');
    } else if (typeof error.error === 'string') {
      this.errorMessage = error.error;
    } else {
      this.errorMessage = 'Something went wrong';
    }
  }
}