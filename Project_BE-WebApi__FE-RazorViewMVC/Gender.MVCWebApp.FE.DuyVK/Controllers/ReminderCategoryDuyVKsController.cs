using Gender.MVCWebApp.FE.DuyVK.Models;
using Gender.Repositories.DuyVK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


namespace Gender.MVCWebApp.FE.DuyVK.Controllers
{
    public class ReminderCategoryDuyVKsController : Controller
    {
        // =================================
        // === Fields
        // =================================

        private readonly string apiEndpoint;

        // =============================
        // === Constructors
        // =============================

        public ReminderCategoryDuyVKsController(IConfiguration configuration)
        {
            apiEndpoint = configuration["APIEndpoint"];
        }

        // =====================================
        // === Index
        // =====================================

        // GET: index page
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] int? currentPage,
            [FromQuery] int? pageSize)
        {
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(apiEndpoint + "ReminderCategoryDuyVK/GetAllPagination" + $"?currentPage={currentPage}&pageSize={pageSize}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PaginationResultResponseVM<List<ReminderCategoryDuyVK>>>(content);

                        if (result != null)
                        {
                            return View("Index", result);
                        }
                    }
                }
            }

            return View("Index", new PaginationResultResponseVM<List<ReminderCategoryDuyVK>>());
        }

        // =====================================
        // === Create
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create()
        {
            var ReminderCategoryDuyVKs = new ReminderCategoryDuyVK()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };





            return View(ReminderCategoryDuyVKs);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReminderCategoryDuyVK ReminderCategoryDuyVK)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    var jsonPayload = JsonConvert.SerializeObject(ReminderCategoryDuyVK);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiEndpoint + "ReminderCategoryDuyVK", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            return View(ReminderCategoryDuyVK);
        }

        // =====================================
        // === Edit
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                // Get the reminder from the id
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(apiEndpoint + "ReminderCategoryDuyVK/" + id))
                {
                    // If unsuccessful, then return to Index
                    if (!response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ReminderCategoryDuyVK>(content);

                    // If dont have result, redirect to index
                    if (result == null) return RedirectToAction(nameof(Index));

                    return View(result);
                }
            }
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReminderCategoryDuyVK ReminderCategoryDuyVK)
        {

            if (id != ReminderCategoryDuyVK.ReminderCategoryDuyVKid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    // token string for authorization
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    using (var response = await httpClient.PutAsJsonAsync(apiEndpoint + "ReminderCategoryDuyVK/", ReminderCategoryDuyVK))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }


            return View(ReminderCategoryDuyVK);
        }

        // =====================================
        // === Delete
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                // token string
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(apiEndpoint + "ReminderCategoryDuyVK/" + id))
                {
                    // If unsuccessful, then return to Index
                    if (!response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ReminderCategoryDuyVK>(content);

                    // If dont have result, redirect to index
                    if (result == null) return RedirectToAction(nameof(Index));

                    return View(result);
                }
            }
        }

        // POST: Delete
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                // token string
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.DeleteAsync(apiEndpoint + "ReminderCategoryDuyVK/" + id))
                {
                    // For bidden
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        TempData["ErrorMessage"] = "You do not have permission to delete this reminder.";
                        return RedirectToAction(nameof(Delete), new { id });
                    }

                    // constraint error
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        TempData["ErrorMessage"] = "You cannot delete this. It's attached to another reminder";
                        return RedirectToAction(nameof(Delete), new { id });
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return RedirectToAction(nameof(Delete), id);
        }
    }
}
