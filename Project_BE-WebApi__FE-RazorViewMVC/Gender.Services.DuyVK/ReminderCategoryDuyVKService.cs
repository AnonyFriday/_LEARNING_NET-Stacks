using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;

namespace Gender.Services.DuyVK
{
    public class ReminderCategoryDuyVKService : IReminderCategoryDuyVKService
    {
        // =============================
        // === Fields
        // =============================

        private readonly ReminderCategoryDuyVKRepository _reminderCategoryDuyVKRepository;

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKService(ReminderCategoryDuyVKRepository reminderCategoryDuyVKRepository)
        {
            _reminderCategoryDuyVKRepository = reminderCategoryDuyVKRepository;
        }

        // =============================
        // === GET
        // =============================

        public async Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetAllAsync(SearchRequest searchRequest)
        {
            return await _reminderCategoryDuyVKRepository.GetAllAsync(
                searchRequest.PageSize.GetValueOrDefault(),
                searchRequest.CurrentPage.GetValueOrDefault());
        }

        public async Task<List<ReminderCategoryDuyVK>> GetAllAsync()
        {
            return await _reminderCategoryDuyVKRepository.GetAllAsync();
        }

        public async Task<ReminderCategoryDuyVK> GetByIdAsync(int id)
        {
            return await _reminderCategoryDuyVKRepository.GetByIdAsync(id);
        }

        // =============================
        // === CREATE, DELETE, UPDATE
        // =============================

        public async Task<bool> CreateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK)
        {
            var affectedRows = await _reminderCategoryDuyVKRepository.CreateAsync(reminderCategoryDuyVK);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK)
        {
            var affectedRows = await _reminderCategoryDuyVKRepository.UpdateAsync(reminderCategoryDuyVK);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reminderCategoryDuyVK = await _reminderCategoryDuyVKRepository.GetByIdAsync(id);
            if (reminderCategoryDuyVK == null) return false;

            return await _reminderCategoryDuyVKRepository.RemoveAsync(reminderCategoryDuyVK);
        }


    }
}
