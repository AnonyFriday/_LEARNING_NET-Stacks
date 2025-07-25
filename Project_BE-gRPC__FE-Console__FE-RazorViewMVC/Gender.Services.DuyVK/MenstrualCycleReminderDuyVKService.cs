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

        private readonly IUnitOfWork _unitOfWork;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================
        // === Get
        // =============================

        public async Task<MenstrualCycleReminderDuyVK> GetByIdAsync(int id)
        {
            return await _unitOfWork.MenstrualCycleReminderDuyVKRepository.GetByIdAsync(id);
        }

        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetAllAsync(SearchRequest searchRequest)
        {
            return await _unitOfWork.MenstrualCycleReminderDuyVKRepository.GetAllAsync(
                searchRequest.PageSize.GetValueOrDefault(),
                searchRequest.CurrentPage.GetValueOrDefault());
        }

        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchAsync(MenstrualCycleReminderSearchRequest request)
        {
            return await _unitOfWork.MenstrualCycleReminderDuyVKRepository.SearchAsync(
                request.Title,
                request.ImportanceScore,
                request.ColorCode,
                request.PageSize.GetValueOrDefault(),
                request.CurrentPage.GetValueOrDefault());
        }

        public async Task<List<MenstrualCycleReminderDuyVK>> GetAllAsync()
        {
            return await _unitOfWork.MenstrualCycleReminderDuyVKRepository.GetAllAsync();
        }

        // =============================
        // === Create, Update, Delete
        // =============================

        public async Task<bool> CreateAsync(MenstrualCycleReminderDuyVK entity)
        {
            // Prepare create
            _unitOfWork.MenstrualCycleReminderDuyVKRepository.PrepareCreate(entity);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(MenstrualCycleReminderDuyVK entity)
        {
            // Prepare update
            _unitOfWork.MenstrualCycleReminderDuyVKRepository.PrepareUpdate(entity);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.MenstrualCycleReminderDuyVKRepository.GetByIdAsync(id);
            if (entity == null) return false;

            // Prepare remove
            _unitOfWork.MenstrualCycleReminderDuyVKRepository.PrepareRemove(entity);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }


    }
}
