using Gender.Repositories.DuyVK;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK.Interfaces;

namespace Gender.Services.DuyVK
{
    public class MenstrualCycleReminderDuyVKService : IMenstrualCycleReminderDuyVKService
    {
        // =============================
        // === Fields
        // =============================

        private readonly MenstrualCycleReminderDuyVKRepository _menstrualCycleReminderDuyVKRepository;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKService(MenstrualCycleReminderDuyVKRepository menstrualCycleReminderDuyVKRepository) =>
            _menstrualCycleReminderDuyVKRepository = menstrualCycleReminderDuyVKRepository;

        // =============================
        // === Get
        // =============================

        public async Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id)
        {
            return await _menstrualCycleReminderDuyVKRepository.GetByIdAsync(id);
        }

        public async Task<List<MenstrualCycleReminderDuyVK>> GetAllAsync()
        {
            return await _menstrualCycleReminderDuyVKRepository.GetAllAsync();
        }

        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(
            SearchRequest searchRequest)
        {
            return await _menstrualCycleReminderDuyVKRepository.GetAllAsync(
                searchRequest.PageSize.GetValueOrDefault(),
                searchRequest.CurrentPage.GetValueOrDefault());
        }

        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(
            SearchMenstrualCycleReminderRequest searchMenstrualCycleReminderRequest)
        {
            return await _menstrualCycleReminderDuyVKRepository.SearchAsync(
                searchMenstrualCycleReminderRequest.Title,
                searchMenstrualCycleReminderRequest.ImportantScore,
                searchMenstrualCycleReminderRequest.ColorCode,
                searchMenstrualCycleReminderRequest.PageSize.GetValueOrDefault(),
                searchMenstrualCycleReminderRequest.CurrentPage.GetValueOrDefault());
        }

        // =============================
        // === Create, Update, Delete
        // =============================

        public async Task<bool> CreateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy)
        {
            var affectedRows = await _menstrualCycleReminderDuyVKRepository.CreateAsync(menstrualCycleReminderDuy);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy)
        {
            var affectedRows = await _menstrualCycleReminderDuyVKRepository.UpdateAsync(menstrualCycleReminderDuy);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menstrualCycleReminderDuyVK = await _menstrualCycleReminderDuyVKRepository.GetByIdAsync(id);
            if (menstrualCycleReminderDuyVK == null) return false;

            return await _menstrualCycleReminderDuyVKRepository.RemoveAsync(menstrualCycleReminderDuyVK);
        }
    }
}
