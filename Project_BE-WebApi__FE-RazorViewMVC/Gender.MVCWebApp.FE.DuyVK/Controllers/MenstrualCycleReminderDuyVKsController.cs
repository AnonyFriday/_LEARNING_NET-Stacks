using Gender.MVCWebApp.FE.DuyVK.Models;
using Gender.Repositories.DuyVK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Gender.MVCWebApp.FE.DuyVK.Controllers
{
    public class MenstrualCycleReminderDuyVKsController : Controller
    {
        // =================================
        // === Fields
        // =================================

        private readonly string apiEndpoint;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKsController(IConfiguration configuration)
        {
            apiEndpoint = configuration["APIEndpoint"];
        }

        // =====================================
        // === Index
        // =====================================

        // GET: index page
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index(
            [FromQuery] int? currentPage,
            [FromQuery] int? pageSize)
        {
            using (var httpClient = new HttpClient())
            {
                var tokenString = HttpContext.Request.Cookies["TokenString"];
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(apiEndpoint + "MenstrualCycleReminderDuyVK" + $"?currentPage={currentPage}&pageSize={pageSize}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PaginationResultResponseVM<List<MenstrualCycleReminderDuyVK>>>(content);

                        if (result != null)
                        {
                            return View("Index", result);
                        }
                    }
                }
            }

            return View("Index", new PaginationResultResponseVM<List<MenstrualCycleReminderDuyVK>>());
        }

        // GET: For Searchings
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Search(
            [FromQuery] SearchMenstrualCycleReminderRequestVM searchRequest)
        {
            // Construct full API URL with query string parameters
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(searchRequest.Title)) queryParams.Add($"title={searchRequest.Title.Trim()}");
            if (!string.IsNullOrWhiteSpace(searchRequest.ColorCode)) queryParams.Add($"colorCode={WebUtility.UrlEncode(searchRequest.ColorCode.Trim())}");
            if (searchRequest.ImportantScore.HasValue) queryParams.Add($"importantScore={searchRequest.ImportantScore}");
            if (searchRequest.CurrentPage.HasValue) queryParams.Add($"currentPage={searchRequest.CurrentPage}");
            if (searchRequest.PageSize.HasValue) queryParams.Add($"pageSize={searchRequest.PageSize}");

            string queryString = string.Join("&", queryParams);
            string requestUrl = apiEndpoint + "MenstrualCycleReminderDuyVK/searchGet";
            if (!string.IsNullOrEmpty(queryString))
                requestUrl += "?" + queryString;

            using var httpClient = new HttpClient();
            var tokenString = HttpContext.Request.Cookies["TokenString"];
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

            var response = await httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PaginationResultResponseVM<List<MenstrualCycleReminderDuyVK>>>(content);
                return View("Index", result);
            }

            return View("Index", new PaginationResultResponseVM<List<MenstrualCycleReminderDuyVK>>());
        }

        // GET: Get all categories for dropdown
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<List<ReminderCategoryDuyVK>> GetReminderCategories()
        {
            var ReminderCategoryDuyVKs = new List<ReminderCategoryDuyVK>();
            using (var httpClient = new HttpClient())
            {

                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                using (var response = await httpClient.GetAsync(apiEndpoint + "ReminderCategoryDuyVK/GetAll"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        ReminderCategoryDuyVKs = JsonConvert.DeserializeObject<List<ReminderCategoryDuyVK>>(content);
                    }
                }
            }

            return ReminderCategoryDuyVKs;
        }

        // =====================================
        // === Create
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create()
        {
            var MenstrualCycleReminderDuyVKs = new MenstrualCycleReminderDuyVK()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ReminderDate = DateTime.Now,
                SentAt = DateTime.Now,
            };

            // Get categories
            var reminderCategories = await this.GetReminderCategories();
            ViewBag.ReminderCategoryDuyVKid = new SelectList(reminderCategories, "ReminderCategoryDuyVKid", "Code");

            return View(MenstrualCycleReminderDuyVKs);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenstrualCycleReminderDuyVK MenstrualCycleReminderDuyVK)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);

                    var jsonPayload = JsonConvert.SerializeObject(MenstrualCycleReminderDuyVK);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiEndpoint + "MenstrualCycleReminderDuyVK", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            // Get categories
            var reminderCategories = await this.GetReminderCategories();
            ViewBag.ReminderCategoryDuyVKid = new SelectList(reminderCategories, "ReminderCategoryDuyVKid", "Code");

            return View(MenstrualCycleReminderDuyVK);
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

                using (var response = await httpClient.GetAsync(apiEndpoint + "MenstrualCycleReminderDuyVK/" + id))
                {
                    // If unsuccessful, then return to Index
                    if (!response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<MenstrualCycleReminderDuyVK>(content);

                    // If dont have result, redirect to index
                    if (result == null) return RedirectToAction(nameof(Index));

                    // Get categories
                    var reminderCategories = await this.GetReminderCategories();
                    ViewBag.ReminderCategoryDuyVKid = new SelectList(reminderCategories, "ReminderCategoryDuyVKid", "Code");
                    return View(result);
                }
            }
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenstrualCycleReminderDuyVK MenstrualCycleReminderDuyVK)
        {

            if (id != MenstrualCycleReminderDuyVK.MenstrualCycleReminderDuyVKid)
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

                    using (var response = await httpClient.PutAsJsonAsync(apiEndpoint + "MenstrualCycleReminderDuyVK/", MenstrualCycleReminderDuyVK))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            return View(MenstrualCycleReminderDuyVK);
        }


        // =====================================
        // === View Details
        // =====================================

        // GET by detail
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                // token string
                var tokenString = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "TokenString").Value;
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                using (var response = await httpClient.GetAsync(apiEndpoint + "MenstrualCycleReminderDuyVK/" + id))
                {
                    // If unsuccessful, then return to Index
                    if (!response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<MenstrualCycleReminderDuyVK>(content);

                    return View(result);
                }
            }
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

                using (var response = await httpClient.GetAsync(apiEndpoint + "MenstrualCycleReminderDuyVK/" + id))
                {
                    // If unsuccessful, then return to Index
                    if (!response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<MenstrualCycleReminderDuyVK>(content);

                    // If dont have result, redirect to index
                    if (result == null) return RedirectToAction(nameof(Index));

                    // Get categories
                    var reminderCategories = await this.GetReminderCategories();
                    ViewBag.ReminderCategoryDuyVKid = new SelectList(reminderCategories, "ReminderCategoryDuyVKid", "Code");

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

                using (var response = await httpClient.DeleteAsync(apiEndpoint + "MenstrualCycleReminderDuyVK/" + id))
                {
                    // For bidden
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        TempData["ErrorMessage"] = "You do not have permission to delete this reminder.";
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
