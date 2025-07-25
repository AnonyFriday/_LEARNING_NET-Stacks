using Gender.Repositories.DuyVK.ModelExtensions;
using Gender.Repositories.DuyVK.Models;
using Gender.Services.DuyVK;
using HotChocolate.Authorization;

namespace Gender.GraphQL.APIServices.DuyVK.GraphQLs
{
    public class Mutations
    {
        // =============================
        // === Fields
        // =============================

        private readonly IServiceProviders _serviceProviders;

        // =============================
        // === Constructors
        // =============================

        public Mutations(IServiceProviders serviceProvider)
        {
            _serviceProviders = serviceProvider;
        }

        // =============================
        // === Authentication vs Authorization
        // =============================

        // AUTHENTICATION: Login with username and password
        public async Task<string?> Login(LoginRequest loginRequest)
        {
            var user = await _serviceProviders.UserAccountService.GetUserAccountAsync(loginRequest.UserName, loginRequest.Password);
            if (user == null) return null;

            var token = _serviceProviders.UserAccountService.GenerateJSONWebToken(user);
            if (string.IsNullOrEmpty(token)) return null;

            return token;
        }

        // =============================
        // === MenstrualCycleReminderDuyVK 
        // =============================

        // CREATE: new Menstrual Cycle Reminder
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<bool> CreateMenstrualCycleReminderDuyVK(MenstrualCycleReminderDuyVK reminder)
        {
            try
            {
                return await _serviceProviders.MenstrualCycleReminderDuyVKService.CreateAsync(reminder);
            }
            catch
            {
                return false;
            }
        }

        // UPDATE: new Menstrual Cycle Reminder
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<bool> UpdateMenstrualCycleReminderDuyVK(MenstrualCycleReminderDuyVK reminder)
        {
            try
            {
                return await _serviceProviders.MenstrualCycleReminderDuyVKService.UpdateAsync(reminder);
            }
            catch
            {
                return false;
            }
        }

        // DELETE: Menstrual Cycle Reminder by ID
        [Authorize(Roles = new string[] { "1" })]
        public async Task<bool> DeleteMenstrualCycleReminderDuyVK(int id)
        {
            try
            {
                return await _serviceProviders.MenstrualCycleReminderDuyVKService.DeleteAsync(id);
            }
            catch
            {
                return false;
            }
        }

        // =============================
        // === ReminderCategoryDuyVK
        // =============================

        // CREATE: new Reminder Category
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<bool> CreateReminderCategoryDuyVK(ReminderCategoryDuyVK category)
        {
            try
            {
                return await _serviceProviders.ReminderCategoryDuyVKService.CreateAsync(category);
            }
            catch
            {
                return false;
            }
        }

        // UPDATE: existing Reminder Category
        [Authorize(Roles = new string[] { "1", "2" })]
        public async Task<bool> UpdateReminderCategoryDuyVK(ReminderCategoryDuyVK category)
        {
            try
            {
                return await _serviceProviders.ReminderCategoryDuyVKService.UpdateAsync(category);
            }
            catch
            {
                return false;
            }
        }


        // DELETE: Reminder Category by ID
        [Authorize(Roles = new string[] { "1" })]
        public async Task<bool> DeleteReminderCategoryDuyVK(int id)
        {
            try
            {
                return await _serviceProviders.ReminderCategoryDuyVKService.DeleteAsync(id);
            }
            catch
            {
                return false;
            }
        }
    }
}
