using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpenseTracker.Api.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Dashboard endpoints for financial summaries, charts, and recent transactions.")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        [SwaggerOperation(
            Summary = "Get dashboard summary",
            Description = "Returns total income, total expenses, current balance, and total number of transactions for the current user."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Dashboard summary returned successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _dashboardService.GetSummaryAsync();
            return Ok(summary);
        }

        [HttpGet("recent-transactions")]
        [SwaggerOperation(
            Summary = "Get recent transactions",
            Description = "Returns the most recent transactions for the current user. Default count is 5."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Recent transactions returned successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> GetRecentTransactions([FromQuery] int count = 5)
        {
            var transactions = await _dashboardService.GetRecentTransactionsAsync(count);
            return Ok(transactions);
        }

        [HttpGet("expenses-by-category")]
        [SwaggerOperation(
            Summary = "Get expenses by category",
            Description = "Returns total expense amount grouped by category for the current user."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Expenses by category returned successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> GetExpensesByCategory()
        {
            var result = await _dashboardService.GetExpensesByCategoryAsync();
            return Ok(result);
        }

        [HttpGet("monthly-income-expense")]
        [SwaggerOperation(
            Summary = "Get monthly income and expenses",
            Description = "Returns monthly income and expense totals for a specific year. If year is not provided, the current year is used."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Monthly income and expense data returned successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> GetMonthlyIncomeExpense([FromQuery] int? year)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;

            var result = await _dashboardService.GetMonthlyIncomeExpenseAsync(selectedYear);
            return Ok(result);
        }

        [SwaggerOperation(
                Summary = "Get top category",
                Description = "Returns the category with the highest total expenses for the current user."
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Top category returned successfully")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        [HttpGet("top-category")]
        public async Task<IActionResult> GetTopCategory()
        {
            var result = await _dashboardService.GetTopCategoryAsync();
            return Ok(result);
        }
}