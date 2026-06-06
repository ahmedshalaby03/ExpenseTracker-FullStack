using ExpenseTracker.Application.DTOs.Transactions;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Application.DTOs.Common;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public TransactionService(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResultDto<TransactionDto>> GetAllAsync(TransactionFilterDto filter)
        {
            var userId = _currentUserService.UserId;

            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(t =>
                    t.Description!.Contains(filter.Search) ||
                    t.Category.Name.Contains(filter.Search));
            }

            if (filter.FromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(t => t.TransactionDate <= filter.ToDate.Value);

            if (filter.Type.HasValue)
                query = query.Where(t => t.Type == filter.Type.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

            if (filter.PaymentMethod.HasValue)
                query = query.Where(t => t.PaymentMethod == filter.PaymentMethod.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    PaymentMethod = t.PaymentMethod,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name
                })
                .ToListAsync();

            return new PagedResultDto<TransactionDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<TransactionDto?> GetByIdAsync(int id)
        {
            var userId = _currentUserService.UserId;

            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    PaymentMethod = t.PaymentMethod,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(TransactionDto? Data, IEnumerable<string>? Errors)> CreateAsync(CreateTransactionDto dto)
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return (null, new[] { "User is not authenticated" });

            if (dto.Amount <= 0)
                return (null, new[] { "Amount must be greater than zero" });

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

            if (category is null)
                return (null, new[] { "Category not found" });

            if (category.Type != dto.Type)
                return (null, new[] { "Transaction type must match category type" });

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description,
                TransactionDate = dto.TransactionDate,
                PaymentMethod = dto.PaymentMethod,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var response = new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                PaymentMethod = transaction.PaymentMethod,
                CategoryId = transaction.CategoryId,
                CategoryName = category.Name
            };

            return (response, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> UpdateAsync(int id, UpdateTransactionDto dto)
        {
            var userId = _currentUserService.UserId;

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction is null)
                return (false, new[] { "Transaction not found" });

            if (dto.Amount <= 0)
                return (false, new[] { "Amount must be greater than zero" });

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

            if (category is null)
                return (false, new[] { "Category not found" });

            if (category.Type != dto.Type)
                return (false, new[] { "Transaction type must match category type" });

            transaction.Amount = dto.Amount;
            transaction.Type = dto.Type;
            transaction.Description = dto.Description;
            transaction.TransactionDate = dto.TransactionDate;
            transaction.PaymentMethod = dto.PaymentMethod;
            transaction.CategoryId = dto.CategoryId;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, IEnumerable<string>? Errors)> DeleteAsync(int id)
        {
            var userId = _currentUserService.UserId;

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction is null)
                return (false, new[] { "Transaction not found" });

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
