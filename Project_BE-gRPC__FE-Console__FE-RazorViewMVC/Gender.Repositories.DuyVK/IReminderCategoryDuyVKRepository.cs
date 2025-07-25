using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Repositories.DuyVK
{
    public interface IReminderCategoryDuyVKRepository : IGenericRepository<ReminderCategoryDuyVK>
    {
        Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetAllAsync(int pageSize, int currentPage);
    }
}