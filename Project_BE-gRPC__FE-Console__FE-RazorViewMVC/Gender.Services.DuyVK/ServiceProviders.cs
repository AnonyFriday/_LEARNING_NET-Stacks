using Gender.Services.DuyVK.Interfaces;

namespace Gender.Services.DuyVK
{
    public interface IServiceProviders
    {
        ISystemUserAccountService UserAccountService { get; }
        IMenstrualCycleReminderDuyVKService MenstrualCycleReminderDuyVKService { get; }
        IReminderCategoryDuyVKService ReminderCategoryDuyVKService { get; }
    }

    public class ServiceProviders : IServiceProviders
    {
        // ==============================
        // === Constructors
        // ==============================

        public ServiceProviders(
            ISystemUserAccountService userAccountService,
            IMenstrualCycleReminderDuyVKService menstrualCycleReminderDuyVKService,
            IReminderCategoryDuyVKService reminderCategoryDuyVKService)
        {
            UserAccountService = userAccountService;
            MenstrualCycleReminderDuyVKService = menstrualCycleReminderDuyVKService;
            ReminderCategoryDuyVKService = reminderCategoryDuyVKService;
        }

        // ==============================
        // === Fields
        // ==============================

        public ISystemUserAccountService UserAccountService { get; }
        public IMenstrualCycleReminderDuyVKService MenstrualCycleReminderDuyVKService { get; }
        public IReminderCategoryDuyVKService ReminderCategoryDuyVKService { get; }
    }
}
