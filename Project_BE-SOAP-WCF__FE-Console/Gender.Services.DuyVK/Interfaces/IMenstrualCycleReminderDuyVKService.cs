using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Services.DuyVK.Interfaces
{
    public interface IMenstrualCycleReminderDuyVKService
    {
        Task<List<MenstrualCycleReminderDuyVK>> GetAllAsync();
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(SearchRequest searchRequest);
        Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id);
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(MenstrualCycleReminderSearchRequest searchMenstrualCycleReminderRequest);

        Task<bool> CreateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy);
        Task<bool> UpdateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy);
        Task<bool> DeleteAsync(int id);
    }
}
