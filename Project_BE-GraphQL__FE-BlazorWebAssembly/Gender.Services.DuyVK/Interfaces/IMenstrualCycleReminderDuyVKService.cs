using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Services.DuyVK.Interfaces
{
    public interface IMenstrualCycleReminderDuyVKService
    {
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(SearchRequest searchRequest);
        Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id);
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(SearchMenstrualCycleReminderRequest searchMenstrualCycleReminderRequest);

        Task<bool> CreateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy);
        Task<bool> UpdateAsync(MenstrualCycleReminderDuyVK menstrualCycleReminderDuy);
        Task<bool> DeleteAsync(int id);
    }
}
