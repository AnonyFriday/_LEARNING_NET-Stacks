using Gender.BusinessObject.Shared.Models.DuyVK.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gender.ReminderCategoryDuyVKs.Microservices.DuyVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderCategoryDuyVKController : ControllerBase
    {
        // =============================
        // === Fields
        // =============================

        private static readonly List<ReminderCategoryDuyVK> _categories = new()
        {
            new ReminderCategoryDuyVK()
                {
                    ReminderCategoryDuyVKid = 1,
                    Code = "CAT001",
                    Name = "Health",
                    Description = "Health related reminders",
                    IsActive = true,
                    PriorityLevel = 1,
                    DefaultOffset = 30,
                    ColorCode = "#FF5733",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new ReminderCategoryDuyVK()
                {
                    ReminderCategoryDuyVKid = 2,
                    Code = "CAT002",
                    Name = "Work",
                    Description = "Work related reminders",
                    IsActive = true,
                    PriorityLevel = 2,
                    DefaultOffset = 15,
                    ColorCode = "#33FF57",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
        };

        // =============================
        // === Constructor
        // =============================

        public ReminderCategoryDuyVKController()
        {
        }

        // =============================
        // === Methods
        // =============================

        // GET: api/<ReminderCategoryDuyVKController>
        [HttpGet]
        public ActionResult<List<ReminderCategoryDuyVK>> Get()
        {
            return _categories;
        }

        // GET api/<ReminderCategoryDuyVKController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReminderCategoryDuyVKController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReminderCategoryDuyVKController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReminderCategoryDuyVKController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
