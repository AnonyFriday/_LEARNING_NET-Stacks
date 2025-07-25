using MenstrualCycleReminderDuyVKServiceReference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReminderCategoryDuyVKServiceReference;
using System.ServiceModel;

namespace Gender.SoapClients.MVCWebApp.DuyVK.Controllers
{
    public class MenstrualCycleReminderDuyVKsController : Controller
    {
        // =================================
        // === Fields
        // =================================

        private ReminderCategoryDuyVKSoapServiceClient _categoryClient;
        private MenstrualCycleReminderDuyVKSoapServiceClient _reminderClient;

        // =============================
        // === Constructors
        // =============================

        public MenstrualCycleReminderDuyVKsController(
            ReminderCategoryDuyVKSoapServiceClient reminderCategoryDuyVKSoapServiceClient,
            MenstrualCycleReminderDuyVKSoapServiceClient menstrualCycleReminderDuyVKSoapServiceClient
            )
        {
            _categoryClient = reminderCategoryDuyVKSoapServiceClient;
            _reminderClient = menstrualCycleReminderDuyVKSoapServiceClient;
        }

        // =====================================
        // === Index
        // =====================================

        // GET: index page
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index(int? currentPage, int? pageSize)
        {
            try
            {
                var req = new SearchRequest
                {
                    CurrentPage = currentPage ?? 1,
                    PageSize = pageSize ?? 10
                };

                var paged = await _reminderClient.GetAllPagedAsync(req);
                return View(paged);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();              // 401
            }
        }

        // GET: For Searchings
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Search([FromQuery] MenstrualCycleReminderSearchRequest searchRequest)
        {
            try
            {
                // Build the SOAP request object
                var req = new MenstrualCycleReminderSearchRequest
                {
                    CurrentPage = searchRequest.CurrentPage ?? 1,
                    PageSize = searchRequest.PageSize ?? 10,
                    Title = searchRequest.Title,
                    ColorCode = searchRequest.ColorCode,
                    ImportanceScore = searchRequest.ImportanceScore
                };

                // Call the generated client
                var paged = await _reminderClient.SearchAsync(req);

                // Render using the same Index view
                return View("Index", paged);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                // token missing or invalid → force login
                return Challenge();
            }
            catch (FaultException fe)
            {
                // other SOAP fault
                ModelState.AddModelError("", $"Service error: {fe.Message}");
                return View("Index", new PaginationResultResponseOfArrayOfMenstrualCycleReminderDuyVK());
            }
            catch (Exception ex)
            {
                // fallback
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
                return View("Index", new PaginationResultResponseOfArrayOfMenstrualCycleReminderDuyVK());
            }
        }

        // =====================================
        // === Create
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create()
        {
            var model = new MenstrualCycleReminderDuyVK
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ReminderDate = DateTime.Now,
                SentAt = DateTime.Now
            };

            var cats = await _categoryClient.GetAllAsync();
            ViewBag.ReminderCategoryDuyVKid = new SelectList(cats, "ReminderCategoryDuyVKid", "Code");
            return View(model);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create(MenstrualCycleReminderDuyVK m)
        {
            if (!ModelState.IsValid)
            {
                // reload categories
                var cats = await _categoryClient.GetAllAsync();
                ViewBag.ReminderCategoryDuyVKid = new SelectList(cats, "ReminderCategoryDuyVKid", "Code");
                return View(m);
            }

            try
            {
                var newId = await _reminderClient.CreateAsync(m);
                return RedirectToAction(nameof(Index));
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();    // or RedirectToLogin
            }
        }

        // =====================================
        // === Edit
        // =====================================

        // GET
        [HttpGet]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _reminderClient.GetByIdAsync(id);
                var cats = await _categoryClient.GetAllAsync();
                ViewBag.ReminderCategoryDuyVKid = new SelectList(cats, "ReminderCategoryDuyVKid", "Code");
                return View(item);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(int id, MenstrualCycleReminderDuyVK m)
        {
            if (id != m.MenstrualCycleReminderDuyVKid)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(m);

            try
            {
                await _reminderClient.UpdateAsync(m);
                return RedirectToAction(nameof(Index));
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();
            }
        }

        // =====================================
        // === Delete
        // =====================================

        // GET: Delete
        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id.HasValue == false)
                    return Challenge();

                var item = await _reminderClient.GetByIdAsync(id.Value);
                var cats = await _categoryClient.GetAllAsync();
                ViewBag.ReminderCategoryDuyVKid = new SelectList(cats, "ReminderCategoryDuyVKid", "Code");
                return View(item);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Delete
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _reminderClient.GetByIdAsync(id);
                var deletedId = await _reminderClient.DeleteAsync(id);

                if (deletedId == 0)
                {
                    ModelState.AddModelError("", "Delete failed. Item not found.");
                    return View(item);
                }

                var cats = await _categoryClient.GetAllAsync();
                ViewBag.ReminderCategoryDuyVKid = new SelectList(cats, "ReminderCategoryDuyVKid", "Code");
                return RedirectToAction(nameof(Index));
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                return Challenge();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
