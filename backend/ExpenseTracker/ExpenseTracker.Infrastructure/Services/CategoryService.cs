using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        public AppDbContext _context { get; }
        public ICurrentUserService _currentUserService { get; }
        public CategoryService(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CategoryFilterDto filter)
        {
            var userId = _currentUserService.UserId;

            var query = _context.Categories
                .Where(c => c.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(c => c.Name.Contains(filter.Search));
            }

            if (filter.Type.HasValue)
            {
                query = query.Where(c => c.Type == filter.Type.Value);
            }

            return await query
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    Icon = c.Icon,
                    Color = c.Color,
                    TransactionsCount = c.Transactions.Count,
                    TotalAmount = c.Transactions.Sum(t => t.Amount)
                })
                .ToListAsync();
        }


        public Task<CategoryDto?> GetByIdAsync(int id)
        {
            var user = _currentUserService.UserId;

            return _context.Categories
                .Where(c => c.UserId == user && c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    Icon = c.Icon,
                    Color = c.Color
                })
                .FirstOrDefaultAsync();
        }
        public async Task<CategorySummaryDto> GetSummaryAsync()
        {
            var userId = _currentUserService.UserId;

            var categories = _context.Categories
                .Where(c => c.UserId == userId);

            var totalCategories = await categories.CountAsync();

            var expenseCount = await categories
                .CountAsync(c => c.Type == TransactionType.Expense);

            var incomeCount = await categories
                .CountAsync(c => c.Type == TransactionType.Income);

            var usedCategories = await categories
                .CountAsync(c => c.Transactions.Any());

            var utilization = totalCategories > 0
                ? Math.Round((decimal)usedCategories / totalCategories * 100, 2)
                : 0;

            return new CategorySummaryDto
            {
                ExpenseCategoriesCount = expenseCount,
                IncomeCategoriesCount = incomeCount,
                UsedCategoriesCount = usedCategories,
                TotalCategoriesCount = totalCategories,
                UtilizationPercentage = utilization
            };
        }

        public async Task<(CategoryDto? Data, IEnumerable<string>? Errors)> CreateAsync(CreateCategoryDto dto)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return (null, new[] { "User is not authenticated" });

            if (string.IsNullOrWhiteSpace(dto.Name))
                return (null, new[] { "Category name is required" });

            var nameExists = await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == dto.Name && c.Type == dto.Type);

            if (nameExists)
                return (null, new[] { "Category already exists" });

            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                UserId = userId,
                Color = dto.Color,
                Icon = dto.Icon
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type,
                Color = dto.Color,
                Icon = dto.Icon
            };

            return (response, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var userId = _currentUserService.UserId;

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category is null)
                return (false, new[] { "Category not found" });

            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, new[] { "Category name is required" });

            var nameExists = await _context.Categories
                .AnyAsync(c =>
                    c.Id != id &&
                    c.UserId == userId &&
                    c.Name == dto.Name &&
                    c.Type == dto.Type);

            if (nameExists)
                return (false, new[] { "Category already exists" });

            category.Name = dto.Name;
            category.Type = dto.Type;
            category.UpdatedAt = DateTime.UtcNow;
            category.Icon = dto.Icon;
            category.Color = dto.Color;

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteAsync(int id)
        {
            var userId = _currentUserService.UserId;

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category is null)
                return (false, new[] { "Category not found" });

            var hasTransactions = await _context.Transactions
                .AnyAsync(t => t.CategoryId == id && t.UserId == userId);

            if (hasTransactions)
                return (false, new[] { "Cannot delete category because it has transactions" });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return (true, null);
        }
        
    }
}
