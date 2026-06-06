using ExpenseTracker.Application.DTOs.Transactions;
using ExpenseTracker.Application.DTOs.Common;

using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResultDto<TransactionDto>> GetAllAsync(TransactionFilterDto filter);
        Task<TransactionDto?> GetByIdAsync(int id);
        Task<(TransactionDto? Data, IEnumerable<string>? Errors)> CreateAsync(CreateTransactionDto dto);
        Task<(bool Success, IEnumerable<string>? Errors)> UpdateAsync(int id, UpdateTransactionDto dto);
        Task<(bool Success, IEnumerable<string>? Errors)> DeleteAsync(int id);
    }
}
