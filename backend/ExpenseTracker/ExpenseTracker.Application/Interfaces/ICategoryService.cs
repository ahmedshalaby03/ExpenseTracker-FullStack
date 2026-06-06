using ExpenseTracker.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(CategoryFilterDto filter);
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategorySummaryDto> GetSummaryAsync();
        Task<(CategoryDto? Data, IEnumerable<string>? Errors)> CreateAsync(CreateCategoryDto dto);
        Task<(bool Success, IEnumerable<string>? Errors)> UpdateAsync(int id, UpdateCategoryDto dto);
        Task<(bool Success, IEnumerable<string>? Errors)> DeleteAsync(int id);
    }
}
