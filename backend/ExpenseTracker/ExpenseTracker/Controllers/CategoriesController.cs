using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpenseTracker.Api.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Category endpoints for managing user income and expense categories.")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

    [HttpGet]
    [SwaggerOperation(
    Summary = "Get all categories",
    Description = "Returns all categories for the current user with optional search and type filter."
    )]
    public async Task<IActionResult> GetAll([FromQuery] CategoryFilterDto filter)
    {
        var categories = await _categoryService.GetAllAsync(filter);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get category by id",
            Description = "Returns a specific category by id if it belongs to the currently authenticated user."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Category returned successfully")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new category",
            Description = "Creates a new income or expense category for the currently authenticated user. Type: Income = 1, Expense = 2."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Category created successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid category data")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateAsync(dto);

            if (result.Data is null)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(
            Summary = "Update category",
            Description = "Updates an existing category if it belongs to the currently authenticated user."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Category updated successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid category data")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);

            if (!result.Success)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete category",
            Description = "Deletes a category if it belongs to the currently authenticated user. Category cannot be deleted if it has related transactions."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Category deleted successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Category cannot be deleted or invalid request")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authenticated")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Errors);

            return NoContent();
        }
        [HttpGet("summary")]
        [SwaggerOperation(
        Summary = "Get categories summary",
        Description = "Returns expense categories count, income categories count, and category utilization percentage."
        )]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _categoryService.GetSummaryAsync();
            return Ok(summary);
        }
}