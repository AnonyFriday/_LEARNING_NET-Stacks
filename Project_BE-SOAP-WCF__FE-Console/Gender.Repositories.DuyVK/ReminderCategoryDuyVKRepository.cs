using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Repositories.DuyVK
{
    public class ReminderCategoryDuyVKRepository : GenericRepository<ReminderCategoryDuyVK>, IReminderCategoryDuyVKRepository
    {
        // ====================
        // === Constructors
        // ====================

        public ReminderCategoryDuyVKRepository(GenderContext context) : base(context) { }

        // ====================
        // === Methods
        // ====================W

        public async Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetAllAsync(int pageSize, int currentPage)
        {
            IQueryable<ReminderCategoryDuyVK> query = _context.ReminderCategoryDuyVKs;

            var items = await GetAllAsync();

            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            items = items
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .ToList();

            return new PaginationResultResponse<List<ReminderCategoryDuyVK>>()
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
