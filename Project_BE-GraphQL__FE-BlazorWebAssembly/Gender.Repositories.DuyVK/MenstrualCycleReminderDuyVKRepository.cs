using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Microsoft.EntityFrameworkCore;

namespace Gender.Repositories.DuyVK
{
    public class MenstrualCycleReminderDuyVKRepository
        : GenericRepository<MenstrualCycleReminderDuyVK>, IMenstrualCycleReminderDuyVKRepository
    {
        // ====================
        // === Constructors
        // ====================

        public MenstrualCycleReminderDuyVKRepository(GenderContext context) : base(context)
        {

        }

        // ====================
        // === Get
        // ====================

        /// <summary>
        /// Get All Async
        /// </summary>
        /// <returns></returns>
        public override async Task<List<MenstrualCycleReminderDuyVK>> GetAllAsync()
        {
            IQueryable<MenstrualCycleReminderDuyVK> query = _context.MenstrualCycleReminderDuyVKs;

            query = query.Include(x => x.ReminderCategoryDuyVK);

            return await query.ToListAsync() ?? new List<MenstrualCycleReminderDuyVK>();
        }

        /// <summary>
        /// Get All Async With Paging
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(int pageSize, int currentPage)
        {
            IQueryable<MenstrualCycleReminderDuyVK> query = _context.MenstrualCycleReminderDuyVKs;

            var items = await GetAllAsync();

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            items = items
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .OrderBy(x => x.MenstrualCycleReminderDuyVKid)
                .ToList();

            return new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>()
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = items
            };
        }

        /// <summary>
        /// Get By Id Async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id)
        {
            var menstrualCycleReminder = await _context.MenstrualCycleReminderDuyVKs
                .Include(d => d.ReminderCategoryDuyVK)
                .FirstOrDefaultAsync(m => m.MenstrualCycleReminderDuyVKid == id);
            return menstrualCycleReminder;
        }

        // ====================
        // === Search
        // ====================

        /// <summary>
        /// Search 3 fields
        /// </summary>
        /// <param name="title"></param>
        /// <param name="importantScore"></param>
        /// <param name="colorCode"></param>
        /// <returns></returns>

        public async Task<List<MenstrualCycleReminderDuyVK>> SearchAsync(
            string? title, double? importantScore, string? colorCode)
        {
            title = title?.Trim().ToLower();
            colorCode = colorCode?.Trim().ToLower();

            var reminders = await _context.MenstrualCycleReminderDuyVKs
                .Include(a => a.ReminderCategoryDuyVK)
                .Where(d => (
                    (string.IsNullOrEmpty(title) || d.Title.ToLower().Contains(title))
                    && (string.IsNullOrEmpty(colorCode) || d.ReminderCategoryDuyVK.ColorCode.ToLower().Contains(colorCode))
                    && (!importantScore.HasValue || d.ImportanceScore == importantScore))
                ).ToListAsync();

            return reminders;
        }

        /// <summary>
        /// Search with paging
        /// </summary>
        /// <param name="title"></param>
        /// <param name="importantScore"></param>
        /// <param name="colorCode"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(
            string? title, double? importantScore, string? colorCode,
            int pageSize, int currentPage)
        {
            var items = await SearchAsync(title, importantScore, colorCode);
            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Paging
            items = items
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .ToList();

            return new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>()
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = items,
            };
        }
    }
}
