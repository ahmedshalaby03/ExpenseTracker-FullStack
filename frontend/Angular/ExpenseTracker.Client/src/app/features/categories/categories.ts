import { Component, OnInit, inject, ChangeDetectorRef  } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin, of, combineLatest} from 'rxjs';
import { catchError, take, filter } from 'rxjs/operators';
import { CategoryService } from '../../core/services/category';
import { Category, CategorySummary } from '../../core/models/category.models';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, CurrencyPipe],
  templateUrl: './categories.html',
  styleUrl: './categories.css'
})
export class CategoriesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private categoryService = inject(CategoryService);

  categories: Category[] = [];
  summary: CategorySummary | null = null;

  isLoading = false;
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  selectedCategoryId: number | null = null;
  searchTerm = '';

  categoryForm = this.fb.group({
    name: ['', [Validators.required]],
    type: [2, [Validators.required]],
    icon: ['utensils'],
    color: ['#2563EB']
  });

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    combineLatest({
      categories: this.categoryService.getAll(this.searchTerm),
      summary: this.categoryService.getSummary()
    }).pipe(
      filter(data => data.categories !== null && data.summary !== null),
      take(1)
    ).subscribe({
      next: (data) => {
        this.categories = data.categories ?? [];
        this.summary = data.summary;
        this.isLoading = false;
        this.cdr.detectChanges(); // ← هنا
      },
      error: () => {
        this.errorMessage = 'Failed to load categories';
        this.isLoading = false;
        this.cdr.detectChanges(); // ← وهنا
      }
    });
  }

  private refreshData(): void {
    combineLatest({
      categories: this.categoryService.getAll(this.searchTerm),
      summary: this.categoryService.getSummary()
    }).pipe(
      filter(data => data.categories !== null && data.summary !== null),
      take(1)
    ).subscribe({
      next: (data) => {
        this.categories = data.categories ?? [];
        this.summary = data.summary;
        this.cdr.detectChanges(); // ← وهنا
      },
      error: () => {
        this.errorMessage = 'Failed to refresh data';
        this.cdr.detectChanges();
      }
    });
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    const request = {
      name: this.categoryForm.value.name!,
      type: Number(this.categoryForm.value.type),
      icon: this.categoryForm.value.icon,
      color: this.categoryForm.value.color
    };

    if (this.selectedCategoryId) {
      this.categoryService.update(this.selectedCategoryId, request).subscribe({
        next: () => {
          this.isSaving = false;
          this.successMessage = 'Category updated successfully';
          this.resetForm();
          this.refreshData();
        },
        error: (error) => this.handleError(error)
      });

      return;
    }

    this.categoryService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.successMessage = 'Category created successfully';
        this.resetForm();
        this.refreshData();
      },
      error: (error) => this.handleError(error)
    });
  }

  edit(category: Category): void {
    this.selectedCategoryId = category.id;

    this.categoryForm.patchValue({
      name: category.name,
      type: category.type,
      icon: category.icon || 'tag',
      color: category.color || '#2563EB'
    });
  }

  delete(category: Category): void {
    const confirmed = confirm(`Delete ${category.name}?`);

    if (!confirmed) return;

    this.categoryService.delete(category.id).subscribe({
      next: () => {
        this.successMessage = 'Category deleted successfully';
        this.refreshData();
      },
      error: (error) => this.handleError(error)
    });
  }

  resetForm(): void {
    this.selectedCategoryId = null;

    this.categoryForm.reset({
      name: '',
      type: 2,
      icon: 'utensils',
      color: '#2563EB'
    });
  }

  search(): void {
    this.loadData();
  }

  getTypeName(type: number): string {
    return type === 1 ? 'Income' : 'Expense';
  }

  getIconClass(icon?: string | null): string {
    switch (icon) {
      case 'wallet':    return 'bi-wallet2';
      case 'briefcase': return 'bi-briefcase';
      case 'utensils':  return 'bi-cup-hot';
      case 'bus':       return 'bi-bus-front';
      case 'bag':       return 'bi-bag';
      case 'receipt':   return 'bi-receipt';
      default:          return 'bi-tag';
    }
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