using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpenseTracker.Api.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Reports endpoints for monthly financial analysis and charts.")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("summary")]
        [SwaggerOperation(
            Summary = "Get monthly report summary",
            Description = "Returns monthly income, expenses, balance, top category, and financial insight."
        )]
        public async Task<IActionResult> GetSummary([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month");

            if (year < 2000)
                return BadRequest("Invalid year");

            var result = await _reportService.GetSummaryAsync(month, year);
            return Ok(result);
        }

        [HttpGet("category-breakdown")]
        [SwaggerOperation(
            Summary = "Get category breakdown",
            Description = "Returns total spending, percentage, and transactions count grouped by category for a selected month."
        )]
        public async Task<IActionResult> GetCategoryBreakdown([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month");

            if (year < 2000)
                return BadRequest("Invalid year");

            var result = await _reportService.GetCategoryBreakdownAsync(month, year);
            return Ok(result);
        }

        [HttpGet("daily-spending")]
        [SwaggerOperation(
            Summary = "Get daily spending fluctuations",
            Description = "Returns daily expense totals for a selected month."
        )]
        public async Task<IActionResult> GetDailySpending([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month");

            if (year < 2000)
                return BadRequest("Invalid year");

            var result = await _reportService.GetDailySpendingAsync(month, year);
            return Ok(result);
        }

        [HttpGet("income-vs-expenses")]
        [SwaggerOperation(
            Summary = "Get yearly income vs expenses",
            Description = "Returns monthly income and expense totals for a selected year."
        )]
        public async Task<IActionResult> GetIncomeVsExpenses([FromQuery] int year)
        {
            if (year < 2000)
                return BadRequest("Invalid year");

            var result = await _reportService.GetIncomeVsExpensesAsync(year);
            return Ok(result);
        }

        [HttpGet("export-csv")]
        [SwaggerOperation(Summary = "Export monthly report as CSV")]
        public async Task<IActionResult> ExportCsv([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month");

            if (year < 2000)
                return BadRequest("Invalid year");

            var fileBytes = await _reportService.ExportCsvAsync(month, year);

            var fileName = $"expense-report-{year}-{month}.csv";

            return File(fileBytes, "text/csv", fileName);
        }
}