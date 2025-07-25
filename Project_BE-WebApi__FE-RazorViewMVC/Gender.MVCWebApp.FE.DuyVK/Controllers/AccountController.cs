using Gender.MVCWebApp.FE.DuyVK.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gender.MVCWebApp.FE.DuyVK.Controllers
{
    public class AccountController : Controller
    {
        // =================================
        // === Fields
        // =================================

        private readonly string apiEndpoint;

        // =============================
        // === Constructors
        // =============================

        public AccountController(IConfiguration configuration)
        {
            apiEndpoint = configuration["APIEndpoint"];
        }

        // =================================
        // === Index
        // =================================

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // =================================
        // === Login
        // =================================

        // GET 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestVM login)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(apiEndpoint + "SystemUserAccounts/Login", login))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            // "token": "avweqcwefwedcea..."
                            var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var tokenString = json.GetValueOrDefault("token");

                            var jwtToken = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;

                            if (jwtToken != null)
                            {
                                var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                                var roleId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, userName),
                                    new Claim(ClaimTypes.Role, roleId),
                                };

                                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                                Response.Cookies.Append("UserName", userName);
                                Response.Cookies.Append("Role", roleId);
                                Response.Cookies.Append("TokenString", tokenString);

                                return RedirectToAction("Index", "MenstrualCycleReminderDuyVKs");
                                //return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure");
            return View();
        }

        // =================================
        // === Logout
        // =================================

        // POST
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("Role");
            Response.Cookies.Delete("TokenString");

            return RedirectToAction("Login");
        }

        // =================================
        // === Forbidden
        // =================================

        // GET
        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
    }
}
