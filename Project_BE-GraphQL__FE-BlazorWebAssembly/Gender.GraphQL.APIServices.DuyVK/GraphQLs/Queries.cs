using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK;
using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gender.GraphQL.APIServices.DuyVK.GraphQLs
{
    public class Queries
    {
        // =============================
        // === Fields
        // =============================

        private readonly IServiceProviders _serviceProviders;

        // =============================
        // === Constructors
        // =============================

        public Queries(IServiceProviders serviceProvider)
        {
            _serviceProviders = serviceProvider;
        }

        // ================================
        // === MenstrualCycleReminderDuyVK
        // ================================

        // GET: Get all with pagination
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> GetMenstrualCycleReminderDuyVKList(SearchRequest searchRequest)
        {
            try
            {
                var result = await _serviceProviders.MenstrualCycleReminderDuyVKService.GetAllAsync(searchRequest);
                return result ?? new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>();
            }
            catch (Exception ex)
            {
                return new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>();
            }
        }

        // GET: Get by id
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<MenstrualCycleReminderDuyVK?> GetMenstrualCycleReminderDuyVKById(int id)
        {
            try
            {
                var result = await _serviceProviders.MenstrualCycleReminderDuyVKService.GetByIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: Search all with pagination and filter
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>> SearchMenstrualCycleReminderDuyVKList(
            SearchMenstrualCycleReminderRequest searchRequest)
        {
            try
            {
                return await _serviceProviders.MenstrualCycleReminderDuyVKService.SearchAsync(searchRequest);
            }
            catch (Exception ex)
            {
                return new PaginationResultResponse<List<MenstrualCycleReminderDuyVK>>();
            }
        }

        // ================================
        // === ReminderCategoryDuyVK
        // ================================

        // GET: Get all with pagination
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<PaginationResultResponse<List<ReminderCategoryDuyVK>>> GetReminderCategoryDuyVKList(
            SearchRequest searchRequest)
        {
            try
            {
                var result = await _serviceProviders.ReminderCategoryDuyVKService.GetAllAsync(searchRequest);
                return result ?? new PaginationResultResponse<List<ReminderCategoryDuyVK>>();
            }
            catch (Exception ex)
            {
                return new PaginationResultResponse<List<ReminderCategoryDuyVK>>();
            }
        }

        // GET: Get all
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<List<ReminderCategoryDuyVK>> GetReminderCategoryDuyVKListAll()
        {
            try
            {
                var result = await _serviceProviders.ReminderCategoryDuyVKService.GetAllAsync();
                return result ?? new List<ReminderCategoryDuyVK>();
            }
            catch (Exception ex)
            {
                return new List<ReminderCategoryDuyVK>();
            }
        }

        // GET: Get by id
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<ReminderCategoryDuyVK?> GetReminderCategoryDuyVKById(int id)
        {
            try
            {
                var result = await _serviceProviders.ReminderCategoryDuyVKService.GetByIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}