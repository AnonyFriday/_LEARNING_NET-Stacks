using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gender.APIServices.BE.DuyVK.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderCategoryDuyVKController : ControllerBase
    {
        // =============================
        // === Fields
        // =============================

        private readonly IReminderCategoryDuyVKService _reminderCategoryDuyVKService;

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKController(IReminderCategoryDuyVKService reminderCategoryDuyVKService)
        {
            _reminderCategoryDuyVKService = reminderCategoryDuyVKService;
        }

        // =============================
        // === GET
        // =============================

        /// <summary>
        /// Get all reminder categories with pagination
        /// </summary>
        [HttpGet("GetAllPagination")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<PaginationResultResponse<List<ReminderCategoryDuyVK>>>> GetAll([FromQuery] SearchRequest searchRequest)
        {
            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 5;

            var result = await _reminderCategoryDuyVKService.GetAllAsync(searchRequest);
            return Ok(result);
        }

        /// <summary>
        /// Get all reminder categories
        /// </summary>
        [HttpGet("GetAll")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<List<ReminderCategoryDuyVK>>> GetAll()
        {
            var result = await _reminderCategoryDuyVKService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a reminder category by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<ReminderCategoryDuyVK>> GetById([FromRoute] int id)
        {
            var category = await _reminderCategoryDuyVKService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Reminder category not found.");

            return Ok(category);
        }

        // =============================
        // === POST, PUT, DELETE
        // =============================

        /// <summary>
        /// Create a new reminder category
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create([FromBody] ReminderCategoryDuyVK category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _reminderCategoryDuyVKService.CreateAsync(category);
                return created ? Ok("Created successfully.") : BadRequest("Failed to create category.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create failed: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while creating the category.");
            }
        }

        /// <summary>
        /// Update a reminder category
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Update([FromBody] ReminderCategoryDuyVK category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _reminderCategoryDuyVKService.UpdateAsync(category);
                return updated ? Ok("Updated successfully.") : BadRequest("Failed to update category.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                return Conflict("Duplicate code.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while creating the category.");
            }
        }

        /// <summary>
        /// Delete a reminder category by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var deleted = await _reminderCategoryDuyVKService.DeleteAsync(id);
                return deleted ? Ok("Deleted successfully.") : NotFound("Category not found or already deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                return Conflict("Reference Key.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while deleting the category.");
            }
        }
    }
}
