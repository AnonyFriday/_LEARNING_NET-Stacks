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

        private readonly IUnitOfWork _unitOfWork;

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================
        // === GET
        // =============================

        public async Task<ReminderCategoryDuyVK> GetByIdAsync(int id)
        {
            return await _unitOfWork.ReminderCategoryDuyVKRepository.GetByIdAsync(id);
        }

        public async Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetAllAsync(SearchRequest searchRequest)
        {
            return await _unitOfWork.ReminderCategoryDuyVKRepository.GetAllAsync(
                searchRequest.PageSize.GetValueOrDefault(),
                searchRequest.CurrentPage.GetValueOrDefault());
        }

        public async Task<List<ReminderCategoryDuyVK>> GetAllAsync()
        {
            return await _unitOfWork.ReminderCategoryDuyVKRepository.GetAllAsync();
        }

        // =============================
        // === CREATE, DELETE, UPDATE
        // =============================

        public async Task<bool> CreateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK)
        {
            _unitOfWork.ReminderCategoryDuyVKRepository.PrepareCreate(reminderCategoryDuyVK);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(ReminderCategoryDuyVK reminderCategoryDuyVK)
        {
            _unitOfWork.ReminderCategoryDuyVKRepository.PrepareUpdate(reminderCategoryDuyVK);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.ReminderCategoryDuyVKRepository.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.ReminderCategoryDuyVKRepository.PrepareRemove(entity);
            var result = await _unitOfWork.SaveChangesWithTransactionAsync();
            return result > 0;
        }
    }
}
