using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Gender.APIServices.BE.DuyVK.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenstrualCycleReminderDuyVKController : ControllerBase
    {
        // =============================
        // === Fields
        // =============================

        private readonly IMenstrualCycleReminderDuyVKService _menstrualCycleReminderDuyVKService;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKController(IMenstrualCycleReminderDuyVKService menstrualCycleReminderDuyVKService)
        {
            _menstrualCycleReminderDuyVKService = menstrualCycleReminderDuyVKService;
        }

        // =============================
        // === GET
        // =============================

        /// <summary>
        /// Get all reminders with pagination
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>> GetAll(
            [FromQuery] SearchRequest searchRequest)
        {
            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 5;

            var result = await _menstrualCycleReminderDuyVKService.GetAllAsync(searchRequest);
            return Ok(result);
        }

        /// <summary>
        /// Get a reminder by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<MenstrualCycleReminderDuyVK>> Get([FromRoute] int id)
        {
            var reminder = await _menstrualCycleReminderDuyVKService.GetByIdAsync(id);
            if (reminder == null)
                return NotFound("Reminder not found.");

            return Ok(reminder);
        }

        // ============================================
        // === SEARCH POST, SEARCH GET, SEARCH ODATA
        // ============================================

        /// <summary>
        /// Search with POST body
        /// </summary>
        [HttpPost("searchPost")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>> SearchPost(
            [FromBody] SearchMenstrualCycleReminderRequest searchRequest)
        {

            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 5;

            var result = await _menstrualCycleReminderDuyVKService.SearchAsync(searchRequest);
            return Ok(result);
        }

        /// <summary>
        /// Search with query string
        /// </summary>
        [HttpGet("searchGet")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>>> SearchGet(
            [FromQuery] SearchMenstrualCycleReminderRequest searchRequest)
        {
            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 5;

            var result = await _menstrualCycleReminderDuyVKService.SearchAsync(searchRequest);
            return Ok(result);
        }

        /// <summary>
        /// Search with query string
        /// </summary>
        [HttpGet("searchOdata")]
        [Authorize(Roles = "1,2")]
        [EnableQuery]
        public async Task<List<MenstrualCycleReminderDuyVK>> SearchOdata()
        {
            return await _menstrualCycleReminderDuyVKService.GetAllAsync();
        }

        /// <summary>
        /// Search with query string
        /// </summary>
        [HttpGet("getOdata")]
        [Authorize(Roles = "1,2")]
        [EnableQuery]
        public async Task<List<MenstrualCycleReminderDuyVK>> GetOdata()
        {
            return await _menstrualCycleReminderDuyVKService.GetAllAsync();
        }

        // =============================
        // === POST, PUT, DELETE
        // =============================

        /// <summary>
        /// Create a new reminder
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create([FromBody] MenstrualCycleReminderDuyVK reminder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool created = await _menstrualCycleReminderDuyVKService.CreateAsync(reminder);
            if (created)
                return Ok("Created successfully.");

            return BadRequest("Failed to create reminder.");
        }

        /// <summary>
        /// Update a reminder
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Update([FromBody] MenstrualCycleReminderDuyVK reminder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool updated = await _menstrualCycleReminderDuyVKService.UpdateAsync(reminder);
            if (updated)
                return Ok("Updated successfully.");

            return BadRequest("Failed to update reminder.");
        }

        /// <summary>
        /// Delete a reminder by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            bool isDeleted = await _menstrualCycleReminderDuyVKService.DeleteAsync(id);
            if (isDeleted)
                return Ok("Deleted successfully.");

            return NotFound("Reminder not found or already deleted.");
        }
    }
}
