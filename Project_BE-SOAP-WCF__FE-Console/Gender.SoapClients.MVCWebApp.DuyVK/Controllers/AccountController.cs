using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ServiceModel;
using SystemUserAccountDuyVKServiceReference;

namespace Gender.SoapClients.MVCWebApp.DuyVK.Controllers
{
    public class AccountController : Controller
    {
        // =================================
        // === Fields
        // =================================

        private SystemUserAccountSoapServiceClient _authClient;

        // =============================
        // === Constructors
        // =============================

        public AccountController(SystemUserAccountSoapServiceClient systemUserAccountSoapServiceClient)
        {
            _authClient = systemUserAccountSoapServiceClient;
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
        public async Task<IActionResult> Login(LoginRequest login)
        {
            if (!ModelState.IsValid)
                return View(login);

            try
            {
                var resp = await _authClient.LoginAsync(login);

                Response.Cookies.Append(
            "TokenString",
            resp.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddSeconds(resp.ExpiresIn)
            });

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(resp.Token);

                var claims = jwt.Claims.Select(c =>
                    new Claim(c.Type, c.Value)
                ).ToList();

                // (Optional) make a “Name” claim if none already:
                if (!claims.Any(c => c.Type == ClaimTypes.Name))
                {
                    var sub = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value
                           ?? jwt.Subject;
                    claims.Add(new Claim(ClaimTypes.Name, sub));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToAction("Index", "MenstrualCycleReminderDuyVKs");
            }
            catch (FaultException fe)
            {
                // SOAP fault (invalid credentials, etc)
                ModelState.AddModelError("", "Invalid username or password.");
                return View(login);
            }
            catch (Exception ex)
            {
                // Other errors
                ModelState.AddModelError("", $"Login failed: {ex.Message}");
                return View(login);
            }
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
