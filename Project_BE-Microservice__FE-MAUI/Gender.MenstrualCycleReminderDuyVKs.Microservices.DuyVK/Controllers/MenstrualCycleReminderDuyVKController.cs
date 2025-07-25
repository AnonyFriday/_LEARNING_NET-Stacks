using Gender.BusinessObject.Shared.Models.DuyVK.Models;
using Gender.Common.Shared.DuyVK;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gender.MenstrualCycleReminderDuyVKs.Microservices.DuyVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenstrualCycleReminderDuyVKController : ControllerBase
    {
        // =============================
        // === Fields
        // =============================

        private readonly ILogger<MenstrualCycleReminderDuyVKController> _logger;
        private readonly IBus _bus;
        private static readonly List<MenstrualCycleReminderDuyVK> _reminders = new()
        {
             new MenstrualCycleReminderDuyVK()
               {
                     MenstrualCycleReminderDuyVKid = 1,
                     ReminderCategoryDuyVKid = 1,
                     Title = "Menstrual Cycle Reminder #1",
                     Note = "Don't forget to track your menstrual cycle.",
                     ReminderDate = DateTime.Now.AddDays(7),
                     ImportanceScore = 0.4,
                     RepeatInterval = 12,
                     SentAt = DateTime.Now.AddDays(-3),
                     IsSent = false,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
               },
               new MenstrualCycleReminderDuyVK()
               {
                     MenstrualCycleReminderDuyVKid = 2,
                     ReminderCategoryDuyVKid = 2,
                     Title = "Menstrual Cycle Reminder #2",
                     Note = "Don't forget to track your menstrual cycle.",
                     ReminderDate = DateTime.Now.AddDays(7),
                     ImportanceScore = 1,
                     RepeatInterval = 12,
                     SentAt = DateTime.Now.AddDays(-3),
                     IsSent = false,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now
               }
        };

        // =============================
        // === Constructor
        // =============================

        public MenstrualCycleReminderDuyVKController(ILogger<MenstrualCycleReminderDuyVKController> logger, IBus bus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        // =============================
        // === Methods
        // =============================

        // GET: api/<MenstrualCycleReminderDuyVKController>
        [HttpGet]
        public ActionResult<List<MenstrualCycleReminderDuyVK>> Get()
        {
            return _reminders;
        }

        // GET api/<MenstrualCycleReminderDuyVKController>/5
        [HttpGet("{id}")]
        public MenstrualCycleReminderDuyVK? Get(int id)
        {
            return _reminders.Find(r => r.MenstrualCycleReminderDuyVKid == id);
        }

        // POST api/<MenstrualCycleReminderDuyVKController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenstrualCycleReminderDuyVK entity)
        {
            if (entity != null)
            {
                // =========== Posting into the existing array============== =============
                _reminders.Add(entity);

                // =========== Demonstration for adding to the rabbit mq bus =============
                // 1. Declare the URL with queue name of the rabbit mq
                // 2. Using the bus
                // 3. send object to the rabbit mq
                // 4. Write the log to console
                // 5. Write the log to the file

                Uri uri = new Uri("rabbitmq://localhost/menstrualCycleReminderDuyVKQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(entity);

                string messageLog = string.Format("[{0}] PUBLISH data to RabbitMQ.menstrualCycleReminderDuyVKQueue: {1}", DateTime.Now, Utilities.ConvertObjectToJSONString(entity));
                Utilities.WriteLoggerFile(messageLog);
                _logger.LogInformation(messageLog);

                return Ok();
            }

            return BadRequest();
        }

        // PUT api/<MenstrualCycleReminderDuyVKController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MenstrualCycleReminderDuyVKController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
