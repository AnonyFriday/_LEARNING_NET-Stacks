using Gender.BusinessObject.Shared.Models.DuyVK.Models;
using Gender.Common.Shared.DuyVK;
using MassTransit;

namespace Gender.ReminderCategoryDuyVKs.Microservices.DuyVK.Consumers
{
    public class MenstrualCycleReminderDuyVKConsumer : IConsumer<MenstrualCycleReminderDuyVK>
    {
        // =================================
        // === Fields
        // =================================

        private readonly ILogger<MenstrualCycleReminderDuyVKConsumer> _logger;

        // =================================
        // === Constructor
        // =================================

        public MenstrualCycleReminderDuyVKConsumer(ILogger<MenstrualCycleReminderDuyVKConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // =================================
        // === Methods
        // =================================

        public async Task Consume(ConsumeContext<MenstrualCycleReminderDuyVK> context)
        {
            var data = context.Message;

            if (data != null)
            {
                string messageLog = string.Format("[{0}] RECEIVE data from RabbitMQ.menstrualCycleReminderDuyVKQueue: {1}", DateTime.Now.ToString(), Utilities.ConvertObjectToJSONString(data));

                Utilities.WriteLoggerFile(messageLog);
                _logger.LogInformation(messageLog);
            }
        }
    }
}
