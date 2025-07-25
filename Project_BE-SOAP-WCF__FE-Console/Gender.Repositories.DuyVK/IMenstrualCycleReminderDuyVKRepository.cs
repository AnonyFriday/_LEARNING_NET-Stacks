using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Repositories.DuyVK
{
    public interface IMenstrualCycleReminderDuyVKRepository : IGenericRepository<MenstrualCycleReminderDuyVK>
    {
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(int pageSize, int currentPage);
        Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id);
        Task<List<MenstrualCycleReminderDuyVK>> SearchAsync(string? title, double? importantScore, string? colorCode);
        Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(string? title, double? importantScore, string? colorCode, int pageSize, int currentPage);
    }
}