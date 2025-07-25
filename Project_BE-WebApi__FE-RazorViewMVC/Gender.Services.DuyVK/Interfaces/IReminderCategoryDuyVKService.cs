
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Services.DuyVK.Interfaces
{
    public interface IReminderCategoryDuyVKService
    {
        public Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetAllAsync(SearchRequest searchRequest);
        public Task<List<ReminderCategoryDuyVK>> GetAllAsync();
        public Task<ReminderCategoryDuyVK> GetByIdAsync(int id);

        public Task<bool> DeleteAsync(int id);
        public Task<bool> UpdateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK);
        public Task<bool> CreateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK);
    }
}
