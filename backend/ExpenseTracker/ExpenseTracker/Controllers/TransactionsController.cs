using ExpenseTracker.Application.DTOs.Transactions;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Transaction endpoints for managing income and expense records.")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all transactions for current user with optional filters")]
    public async Task<IActionResult> GetAll([FromQuery] TransactionFilterDto filter)
    {
        var transactions = await _transactionService.GetAllAsync(filter);
        return Ok(transactions);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get transaction by id")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);

        if (transaction is null)
            return NotFound("Transaction not found");

        return Ok(transaction);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create new transaction")]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var result = await _transactionService.CreateAsync(dto);

        if (result.Data is null)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update transaction")]
    public async Task<IActionResult> Update(int id, UpdateTransactionDto dto)
    {
        var result = await _transactionService.UpdateAsync(id, dto);

        if (!result.Success)
            return BadRequest(result.Errors);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete transaction")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _transactionService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result.Errors);

        return NoContent();
    }
}